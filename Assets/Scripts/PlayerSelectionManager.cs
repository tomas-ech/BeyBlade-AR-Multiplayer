using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcher;


    public int playerNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModel_Text;

    public GameObject[] spinnerModels;
    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;

    public Button nextBtn;
    public Button previousBtn;

    #region UNITY methods

    void Start()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
        playerNumber = 0;
    }

    void Update()
    {
        
    }

    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        playerNumber += 1;
        if (playerNumber >= spinnerModels.Length)
        {
            playerNumber = 0;
        }
        Debug.Log(playerNumber);

        nextBtn.enabled = false;
        previousBtn.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcher, 90, 1f));

        if (playerNumber == 0 || playerNumber == 1)
        {
            playerModel_Text.text = "Attacker";
        }
        else
        {
            playerModel_Text.text = "Defender";
        }
    }

    public void PreviousPlayer()
    {
        playerNumber -= 1;

        if (playerNumber < 0)
        {
            playerNumber = spinnerModels.Length - 1;
        }
        Debug.Log(playerNumber);

        nextBtn.enabled = false;
        previousBtn.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcher, -90, 1f));

        if (playerNumber == 0 || playerNumber == 1)
        {
            playerModel_Text.text = "Attacker";
        }
        else
        {
            playerModel_Text.text = "Defender";
        }
    }

    public void OnSelectButtonClicked()
    {
        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);


        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { {MultiplayerARSpinnerGame.PLAYER_SELECTION_NUMBER, playerNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }
    public void OnReSelectButtonClicked()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }
    
    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion

    #region Private Methods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis*angle);

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        nextBtn.enabled = true;
        previousBtn.enabled = true;
    }

    #endregion
}
