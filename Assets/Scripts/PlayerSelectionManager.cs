using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerSelectionManager : MonoBehaviour
{
    [SerializeField] Button btn_Next;
    [SerializeField] Button btn_Previous;

    public Transform playerSwitcherTransform;

    public int playerSelectionNumber;

    public GameObject[] spinnerTopModels;

    private void Start()
    {
        playerSelectionNumber = 0;
    }

    #region UI Callbacks
    public void NextPlayer()
    {
        playerSelectionNumber++;
        btn_Next.enabled = false;
        btn_Previous.enabled = false;
        if (playerSelectionNumber > spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90.0f, 1.0f));
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber--;
        btn_Next.enabled = false;
        btn_Previous.enabled = false;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90.0f, 1.0f));
    }

    public void OnSelectButtonClick()
    {
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { { ARMulltiplayerBeybladeGame.PLAYER_SELECTIO_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);

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
    }
    #endregion
}
