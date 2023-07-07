using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class SpinningGameManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject uI_InformPanelGo;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchGamesGo;

    void Start()
    {
        uI_InformPanelGo.SetActive(true);
        uI_InformText.text = "¡Searching for Battles!";

    }

    void Update()
    {
        
    }

    #region UI Callbacks
    public void JoinRandomRoom()
    {
        uI_InformText.text = "Searching rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchGamesGo.SetActive(false);
    }

    #endregion

    #region PHOTON Callbacks Methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        uI_InformText.text = message;

        Debug.Log(message);

        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + " Waiting for players";
        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGo, 2f));
        }

        Debug.Log("Joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        uI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count: " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGo, 2f));
    }

    #endregion

    #region PRIVATE Methods

    private void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 100).ToString("D0");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    #endregion
}
