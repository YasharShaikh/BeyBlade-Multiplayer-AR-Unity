using Photon.Pun;
using UnityEngine;



public class PlayerSync : MonoBehaviour, IPunObservable
{
    Rigidbody rigidbody;
    PhotonView photonView;
    Vector3 position;
    Quaternion rotation;
    float angle;
    float distance;
    [SerializeField] bool synchronizeVelocity = true;
    [SerializeField] bool synchronizeAngularVelocity = true;
    [SerializeField] bool isTeleportableEnabled = true;
    [SerializeField] float teleportIfDistanceGreater = 1.0f;

    private GameObject arena;
     

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        position = new Vector3();
        rotation = new Quaternion();
        arena = GameObject.Find("BattleArena");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rigidbody.position = Vector3.MoveTowards(rigidbody.position, position, distance * (1.0f / PhotonNetwork.SerializationRate));
            rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, rotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rigidbody.position - arena.transform.position);
            stream.SendNext(rigidbody.rotation);

            if (synchronizeVelocity)
                stream.SendNext(rigidbody.linearVelocity);

            if (synchronizeAngularVelocity)
                stream.SendNext(rigidbody.angularVelocity);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext() + arena.gameObject.transform.position;
            rotation = (Quaternion)stream.ReceiveNext();

            if(isTeleportableEnabled)
            {
                if(Vector3.Distance(rigidbody.position,position)>teleportIfDistanceGreater)
                {
                    rigidbody.position = position;
                }
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                if (synchronizeVelocity)
                {
                    rigidbody.linearVelocity = (Vector3)stream.ReceiveNext();
                    position += rigidbody.linearVelocity * lag;
                    distance = Vector3.Distance(rigidbody.position, position);
                }

                if (synchronizeAngularVelocity)
                {
                    rigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
                    rotation = Quaternion.Euler(rigidbody.angularVelocity * lag) * rotation;
                    angle = Quaternion.Angle(rigidbody.rotation, rotation);
                }
            }

        }
    }
}
