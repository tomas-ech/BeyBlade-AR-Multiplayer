using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInput;

    #region UNITY methods
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                PhotonNetwork.ConnectUsingSettings();
            }
        }

        else
        {
            Debug.Log("player name is invalid or empty!");
        }
    }

    #endregion

    #region  PHOTON Callbacks Methods
    public override void OnConnected()
    {
        Debug.Log("We connected to internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon server");
    }


    #endregion
}
