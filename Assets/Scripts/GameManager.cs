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
        text_InformationPanel.text = "Searching for room.";
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
        CreateandJoinRoom();
    }
    public override void OnJoinedRoom()
    {
        btn_adjust.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            text_InformationPanel.text = "Joined room: " + PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            text_InformationPanel.text = "Joined room: " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(Deactivate(ui_InformationPanel, 2.0f));
        }
        Debug.Log("Joined Room: " + PhotonNetwork.NickName + " in Room:" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(Deactivate(ui_InformationPanel, 2.0f));
    }


    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadeScene("Scene_Lobby");
    }

    private void CreateandJoinRoom()
    {
        string randomRoomName = "Room" + UnityEngine.Random.Range(1, 1000).ToString("D3");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator Deactivate(GameObject gameobject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameobject.SetActive(false);
    }


    #endregion
}
