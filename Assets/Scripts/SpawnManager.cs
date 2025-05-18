using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public enum RaiseEventCode
{
    PlayerSpawnEventCode = 0
}

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform[] playerSpawnPositions;
    [SerializeField] private GameObject arena;



    private void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCode.PlayerSpawnEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            Vector3 offsetPosition = (Vector3)data[0];
            Quaternion rotation = (Quaternion)data[1];
            int viewID = (int)data[2];
            int playerSelection = (int)data[3];

            if (playerSelection < 0 || playerSelection >= playerPrefabs.Length)
            {
                Debug.LogError("Invalid player selection index received.");
                return;
            }

            Vector3 worldPosition = arena.transform.position + offsetPosition;

            GameObject player = Instantiate(playerPrefabs[playerSelection], worldPosition, rotation);
            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.ViewID = viewID;
            photonView.enabled = true;
        }
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(ARMulltiplayerBeybladeGame.PLAYER_SELECTION_NUMBER, out object playerSelectionNumber))
        {
            int randomSpawnIndex = UnityEngine.Random.Range(0, playerSpawnPositions.Length); // fixed off-by-one
            Vector3 spawnPosition = playerSpawnPositions[randomSpawnIndex].position;

            GameObject player = Instantiate(playerPrefabs[(int)playerSelectionNumber], spawnPosition, Quaternion.identity);
            PhotonView photonView = player.GetComponent<PhotonView>();

            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                    player.transform.position - arena.transform.position, // local offset
                    player.transform.rotation,
                    photonView.ViewID,
                    playerSelectionNumber
                };

                RaiseEventOptions raiseOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions { Reliability = true };

                PhotonNetwork.RaiseEvent((byte)RaiseEventCode.PlayerSpawnEventCode, data, raiseOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate ViewID!");
                Destroy(player);
            }
        }
        else
        {
            Debug.LogWarning("PLAYER_SELECTION_NUMBER custom property not found.");
        }
    }
}
