using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    [SerializeField] Button btn_Next;
    [SerializeField] Button btn_Previous;

    public Transform playerSwitcherTransform;

    public int playerSelectionNumber;

    public GameObject[] spinnerTopModels;

    [Space]
    [SerializeField] TextMeshProUGUI text_playerModeType;
    [SerializeField] GameObject ui_Selection;
    [SerializeField] GameObject ui_AfterSelection;
    private void Start()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false); 
        playerSelectionNumber = 0;
    }

    #region UI Callbacks
    public void NextPlayer()
    {
        playerSelectionNumber++;

        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90.0f, 1.0f));


        UpdatePlayerModeUI();

    }

    public void PreviousPlayer()
    {
        playerSelectionNumber--;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90.0f, 1.0f));
        UpdatePlayerModeUI();
    }

    public void OnSelectButtonClick()
    {
        ui_Selection.SetActive(false);
        ui_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { ARMulltiplayerBeybladeGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);

    }
    public void OnReselectButtonClikced()
    {
        ui_Selection.SetActive(true);
        ui_AfterSelection.SetActive(false);
    }
    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadeScene("Scene_Gameplay");
    }
    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadeScene("Scene_Lobby");
    }
    private void UpdatePlayerModeUI()
    {
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
            text_playerModeType.text = "Attack";
        else
            text_playerModeType.text = "Defence";
    }
    #endregion

    #region Rotation Logic
    private IEnumerator Rotate(Vector3 axis, Transform target, float angle, float duration)
    {
        Quaternion startRotation = target.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(axis * angle);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.rotation = endRotation;

        // Re-enable buttons
        btn_Next.enabled = true;
        btn_Previous.enabled = true;
    }

    #endregion
}
