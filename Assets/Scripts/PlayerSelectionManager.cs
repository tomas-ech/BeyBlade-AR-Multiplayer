using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcher;

    public Button nextBtn;
    public Button previousBtn;

    #region UNITY methods

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region UI Callback Methods
    public void NextPlayer()
    {
        nextBtn.enabled = false;
        previousBtn.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcher, 90, 1f));
    }

    public void PreviousPlayer()
    {
        nextBtn.enabled = false;
        previousBtn.enabled = false;
        StartCoroutine(Rotate(Vector3.up, playerSwitcher, -90, 1f));
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
