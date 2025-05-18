using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class BattleScript : MonoBehaviourPun
{

    [SerializeField] float startSpinSpeed;
    [SerializeField] float currentSpinSpeed;
    [SerializeField] Image img_SpinSpeedbar;
    [SerializeField] TextMeshProUGUI text_SpinSpeedRatio;
    [SerializeField] float common_dmg_coefficiant = 0.04f;
    [SerializeField] float doDmg_coefficient_attacker = 10.0f;
    [SerializeField] float getDmg_coefficient_attacker = 10.0f;
    [SerializeField] float doDmg_coefficient_defender = 0.75f;
    [SerializeField] float getDmg_coefficient_defender = 0.2f;
    [SerializeField] GameObject ui_3d;
    [SerializeField] GameObject ui_DeathPanel;
    GameObject deathPanel;
    public bool isAttacker;
    public bool isDefender;
    bool isDead;
    Rigidbody rigidbody;
    Spinner spinner;



    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        spinner = GetComponent<Spinner>();
        startSpinSpeed = spinner.spinSpeed;
        currentSpinSpeed = startSpinSpeed;

        img_SpinSpeedbar.fillAmount = currentSpinSpeed / startSpinSpeed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckPlayerType();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float speed = gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude;
            float enemySpeed = collision.collider.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude;


            if (speed > enemySpeed)
            {
                float default_DmgAmt = gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude * 3600.0f * common_dmg_coefficiant;

                if (isAttacker)
                {
                    default_DmgAmt *= doDmg_coefficient_attacker;

                }
                else if (isDefender)
                {
                    default_DmgAmt *= doDmg_coefficient_defender;
                }
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {


                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_DmgAmt);
                }

            }
        }
    }


    void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else
        {
            isAttacker = false;
            isDefender = true;
            spinner.spinSpeed = 4400.0f;
            startSpinSpeed = spinner.spinSpeed;
            currentSpinSpeed = spinner.spinSpeed;


            text_SpinSpeedRatio.text = currentSpinSpeed + "/" + startSpinSpeed;
        }

    }

    [PunRPC]
    void DoDamage(float _damage)
    {

        if (!isDead)
        {
            if (isAttacker)
            {
                _damage *= getDmg_coefficient_attacker;
                if(_damage>1000.0f)
                {
                    _damage = 400.0f;
                }
            }
            else if (isDefender)
            {
                _damage *= getDmg_coefficient_defender;
            }


            spinner.spinSpeed -= _damage;
            currentSpinSpeed = spinner.spinSpeed;
            img_SpinSpeedbar.fillAmount = currentSpinSpeed / startSpinSpeed;
            text_SpinSpeedRatio.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;

            if (currentSpinSpeed < 100.0f)
            {
                Die();
            }

        }


    }

    void Die()
    {
        isDead = true;
        GetComponent<MovementController>().enabled = false;
        rigidbody.freezeRotation = false;
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;


        spinner.spinSpeed = 0.0f;

        ui_3d.gameObject.SetActive(false);


        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (deathPanel == null)
        {
            deathPanel = Instantiate(ui_DeathPanel, canvas.transform);
        }
        else
        {
            deathPanel.SetActive(true);
        }

        Text text_respawnTime = deathPanel.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8.0f;

        text_respawnTime.text = respawnTime.ToString(".00");
        while (respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            text_respawnTime = deathPanel.transform.Find("RespawnTimeText").GetComponent<Text>();

            GetComponent<MovementController>().enabled = false;
        }

        deathPanel?.SetActive(false);
        GetComponent<MovementController>().enabled = true;

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void Reborn()
    {
        spinner.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinner.spinSpeed;
        img_SpinSpeedbar.fillAmount = currentSpinSpeed / startSpinSpeed;
        text_SpinSpeedRatio.text = currentSpinSpeed + "/" + startSpinSpeed;

        rigidbody.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        ui_3d.SetActive(true);
        isDead = false;
    }
}
