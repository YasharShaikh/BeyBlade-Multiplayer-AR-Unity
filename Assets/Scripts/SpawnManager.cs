using UnityEngine;
using Photon.Pun; 
public class SpawnManager : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] Transform[] playerSpawnPositions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(ARMulltiplayerBeybladeGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {

                int randomSpawnPoints = UnityEngine.Random.Range(0,playerSpawnPositions.Length-1);
                Vector3 instantiatePositions = playerSpawnPositions[randomSpawnPoints].position;


                PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePositions, Quaternion.identity);
            }
        }

    }
}
