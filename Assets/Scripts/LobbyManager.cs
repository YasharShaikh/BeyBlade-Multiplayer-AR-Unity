using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    [Header("UI References")]
    [SerializeField] private InputField playerNameInputField;
    [SerializeField] private GameObject ui_Login;
    [SerializeField] private GameObject ui_Lobby;
    [SerializeField] private GameObject ui_3DObject;
    [SerializeField] private GameObject ui_ConnectionStatus;
    [SerializeField] private Text text_ConnectionStatus;

    [Header("Scene")]
    [SerializeField] private string sceneLoading;

    private bool firstTimeLogin = false;
    private Photon.Realtime.ClientState lastClientState;

    #region Unity Methods
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lastClientState = PhotonNetwork.NetworkClientState;
        UpdateConnectionStatus();
        LoginToPlayFab();
    }

    private void Update()
    {
        if (lastClientState != PhotonNetwork.NetworkClientState)
        {
            lastClientState = PhotonNetwork.NetworkClientState;
            UpdateConnectionStatus();
        }
    }
    #endregion

    #region PlayFab Login
    private void LoginToPlayFab()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("[PlayFab] Login successful!");

        // Check if player already has a display name
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), accountResult =>
        {
            string displayName = accountResult.AccountInfo.TitleInfo.DisplayName;
            if (string.IsNullOrEmpty(displayName))
            {
                firstTimeLogin = true;
                SetUIState(true, false, false, false); // Show login input
            }
            else
            {
                PhotonNetwork.NickName = displayName;
                SetUIState(false, true, true, false); // Show lobby directly
                ConnectToPhoton();
            }
        }, error =>
        {
            Debug.LogError("[PlayFab] GetAccountInfo failed: " + error.GenerateErrorReport());
        });
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("[PlayFab] Login failed: " + error.GenerateErrorReport());
    }
    #endregion

    #region UI Callbacks
    public void OnEnterButtonClick()
    {
        if (!firstTimeLogin) return;

        string playerName = playerNameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("[LobbyManager] Player Name is invalid");
            return;
        }

        // Save display name to PlayFab
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = playerName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("[PlayFab] Display name set: " + result.DisplayName);
            PhotonNetwork.NickName = result.DisplayName;
            firstTimeLogin = false;
            SetUIState(false, true, true, false);
            ConnectToPhoton();
        }, error =>
        {
            Debug.LogError("[PlayFab] Failed to set display name: " + error.GenerateErrorReport());
        });
    }

    public void OnQuickMatchButtonClick()
    {
        if (!string.IsNullOrEmpty(sceneLoading))
            SceneLoader.Instance.LoadScene(sceneLoading);
        else
            Debug.LogWarning("[LobbyManager] Scene name is not set for quick match.");
    }
    #endregion

    #region Photon Methods
    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnected()
    {
        Debug.Log("[Photon] Connected to server.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[Photon] Connected to master, Player: " + PhotonNetwork.NickName);
        SetUIState(false, true, true, false);
    }
    #endregion

    #region UI Helpers
    private void SetUIState(bool login, bool lobby, bool obj3D, bool connectionStatus)
    {
        ui_Login.SetActive(login);
        ui_Lobby.SetActive(lobby);
        ui_3DObject.SetActive(obj3D);
        ui_ConnectionStatus.SetActive(connectionStatus);
    }

    private void UpdateConnectionStatus()
    {
        text_ConnectionStatus.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
    }
    #endregion
}
