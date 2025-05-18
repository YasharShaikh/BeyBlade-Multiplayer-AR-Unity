using Photon.Pun;
using UnityEngine;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI text_PlayerName;
    private MovementController movementController;

    void Start()
    {
        movementController = GetComponent<MovementController>();

        if (movementController == null)
        {
            Debug.LogError("MovementController component is missing on " + gameObject.name);
            return;
        }

        if (photonView.IsMine)
        {
            EnableMovement(true);
        }
        else
        {
            EnableMovement(false);
        }

        if(photonView.IsMine)
        {
            text_PlayerName.text = "You";
        }
        else
        {
            text_PlayerName.text = photonView.Owner.NickName;
        }

    }

    private void EnableMovement(bool state)
    {
        if (movementController.joystick != null)
        {
            movementController.joystick.enabled = state;
        }
        else
        {
            Debug.LogWarning("Joystick reference is missing on " + gameObject.name);
        }

        movementController.enabled = state;
    }
}
