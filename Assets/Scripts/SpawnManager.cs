using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPositions;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Photon Callbacks Methods

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                Debug.Log("Player number is: " + (int)playerSelectionNumber);

                int randomSpawnPoint = Random.Range(0, spawnPositions.Length -1);
                Vector3 instantiatePosition = spawnPositions[randomSpawnPoint].position;

                PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
            }
            
            
        }
    }

    #endregion
}
