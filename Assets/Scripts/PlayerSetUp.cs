using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetUp : MonoBehaviourPun
{
    public TextMeshProUGUI playerNameText;

    void Start()
    {
        if (photonView.IsMine)
        {
            //the player is local
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);


        }
        else
        {
            //The player is remote
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }

        SetPlayerName();
    }

    private void SetPlayerName()
    {
        if (playerNameText != null)
        {
            if (photonView.IsMine)
            {
                playerNameText.text = "YOU";
                playerNameText.color = Color.red;
            }
            else
            {
                playerNameText.text = photonView.Owner.NickName;
            }

        }
    }
}
