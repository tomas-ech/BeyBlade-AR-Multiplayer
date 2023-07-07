using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInput;
    public GameObject uI_LoginGo; //Go = Gameobject

    [Header("Lobby UI")]
    public GameObject uI_LobbyGo;
    public GameObject uI_3dGo;

    [Header("Connection Status UI")]
    public GameObject uI_ConnectionStatusGo;
    public TextMeshProUGUI connectionsStatusText;
    public bool showStatus;


    #region UNITY methods
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            uI_LoginGo.SetActive(false);
            uI_ConnectionStatusGo.SetActive(false);

            uI_LobbyGo.SetActive(true);
            uI_3dGo.SetActive(true);
        }
        else
        {
            uI_LobbyGo.SetActive(false);
            uI_3dGo.SetActive(false);
            uI_ConnectionStatusGo.SetActive(false);

            uI_LoginGo.SetActive(true);
        }

    }

    void Update()
    {
        if (showStatus)
        {
            connectionsStatusText.text = "Status: " + PhotonNetwork.NetworkClientState;
        }
    }

    #endregion

    #region UI Callback Methods

    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            uI_LobbyGo.SetActive(false);
            uI_3dGo.SetActive(false);
            uI_LoginGo.SetActive(false);

            uI_ConnectionStatusGo.SetActive(true);

            showStatus = true;

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

    public void OnQuickMatchClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection"); 
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

        uI_LoginGo.SetActive(false);
        uI_ConnectionStatusGo.SetActive(false);

        uI_LobbyGo.SetActive(true);
        uI_3dGo.SetActive(true);
    }


    #endregion
}
