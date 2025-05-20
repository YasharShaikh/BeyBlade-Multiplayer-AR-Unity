using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject ui_InformationPanel;
    [SerializeField] TextMeshProUGUI text_InformationPanel;
    [SerializeField] GameObject btn_SearchForGame;
    [SerializeField] GameObject btn_adjust;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui_InformationPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Callbacks
    public void JoinRoomButton()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            text_InformationPanel.text = "Not connected to Photon.";
            Debug.LogWarning("Join failed: Not connected.");
            return;
        }

        text_InformationPanel.text = "Searching for room...";
        PhotonNetwork.JoinRandomRoom();
        btn_SearchForGame.SetActive(false);
    }


    public void QuitMatch()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneLoader.Instance.LoadeScene("Scene_Lobby");
        }

    }
    #endregion


    #region PUN Callbacks

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        text_InformationPanel.text = message;
        CreateAndJoinRoom();
    }
    public override void OnJoinedRoom()
    {
        btn_adjust.SetActive(false);

        text_InformationPanel.text = $"Joined room: {PhotonNetwork.CurrentRoom.Name}";

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            StartCoroutine(DeactivateAfterDelay(ui_InformationPanel, 2.0f));
        }

        Debug.Log($"Joined Room: {PhotonNetwork.NickName} in Room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(DeactivateAfterDelay(ui_InformationPanel, 2.0f));
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        text_InformationPanel.text = $"Disconnected: {cause}";
        btn_SearchForGame.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadeScene("Scene_Lobby");
        btn_SearchForGame.SetActive(true);
        ui_InformationPanel.SetActive(true);
        text_InformationPanel.text = "Ready to join a game.";
    }

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + UnityEngine.Random.Range(1, 1000).ToString("D3");
        RoomOptions roomOptions = new RoomOptions
        {
            IsOpen = true,
            IsVisible = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterDelay(GameObject gameobject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameobject.SetActive(false);
    }


    #endregion
}
