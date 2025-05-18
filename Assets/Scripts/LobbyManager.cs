using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Rendering;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    [SerializeField] InputField playerNameInputField;
    [SerializeField] GameObject ui_Login;
    [SerializeField] GameObject ui_Lobby;
    [SerializeField] GameObject ui_3dObject;
    [SerializeField] GameObject ui_ConnectionStatus;
    [Space]
    [SerializeField] Text text_ConnectionStatus;
    [Space]
    [SerializeField] string Scene_Loading;

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) 
        {
            ui_Login.SetActive(true);
            ui_Lobby.SetActive(true);
            ui_3dObject.SetActive(false);
            ui_ConnectionStatus.SetActive(false);
        }else
        {
            ui_Login.SetActive(true);
            ui_Lobby.SetActive(false);
            ui_3dObject.SetActive(false);
            ui_ConnectionStatus.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        text_ConnectionStatus.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
    }
    #endregion


    #region UI Callbacks
    public void OnEnterButtonClick()
    {
        ui_Login.SetActive(false);
        ui_Lobby.SetActive(false);
        ui_3dObject.SetActive(false);
        ui_ConnectionStatus.SetActive(true);

        string playerName = playerNameInputField.text;
        if(!string.IsNullOrEmpty(playerName))
        {
            ui_Login.SetActive(false);
            ui_Lobby.SetActive(false);
            ui_3dObject.SetActive(false);
            ui_ConnectionStatus.SetActive(true);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();

            }
        }
        else
        {
            Debug.Log("[Lobby Manager] Player Name is invalid");
        }
    }


    public void OnQuickMatchButtonClick()
    {
        SceneLoader.Instance.LoadeScene(Scene_Loading);
    }
    #endregion

    #region Photon callbacks methods
    public override void OnConnected()
    {
        Debug.Log("[Lobby Manager] connected to the internet");
    }   

    public override void OnConnectedToMaster()
    {
        Debug.Log("[Lobby Manager] connected to the internet, Player: " + PhotonNetwork.LocalPlayer.NickName);


        ui_Login.SetActive(false);
        ui_Lobby.SetActive(true);
        ui_3dObject.SetActive(true);
        ui_ConnectionStatus.SetActive(false);
    }

    #endregion
}
