using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleSystem : MonoBehaviour
{
    public SpinnerControl spinnerScript;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar;
    public TextMeshProUGUI spinSpeedText;

    public float damageCoefficient = 0.04f;
    public bool isAttacker;
    public bool isDefender;

    [Header("Player Type Coefficients")]
    public float doDamageCoefficientAttacker = 10f;
    public float getDamagedCoefficientAttacker = 1.2f;

    public float doDamageCoefficientDefender = 0.75f;
    public float getDamagedCoefficientDefender = 0.2f;


    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if (gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;
        }
        else if (gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;

            spinnerScript.spinSpeed = 4400;

            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedText.text = currentSpinSpeed.ToString("F0") + " / " + startSpinSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = other.collider.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("My Speed: " + mySpeed + " --- Other Player Speed: " + otherPlayerSpeed);

            if (mySpeed > otherPlayerSpeed)
            {
                float defaultDamage = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * damageCoefficient;

                if (isAttacker)
                {
                    defaultDamage *= doDamageCoefficientAttacker;
                }
                else if (isDefender)
                {
                    defaultDamage *= doDamageCoefficientDefender;
                }

                if (other.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {

                    //Aplicar daño al jugador mas lento
                    other.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defaultDamage);
                }

                Debug.Log("You damage the other player");                
            }
            
        }
    }

    [PunRPC]
    public void DoDamage(float amount)
    {
        if (isAttacker)
        {
            amount *= getDamagedCoefficientAttacker;
        }
        else if (isDefender)
        {
            amount *= getDamagedCoefficientDefender;
        }

        spinnerScript.spinSpeed -= amount;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedText.text = currentSpinSpeed + " / " + startSpinSpeed;

        if (currentSpinSpeed <= 0)
        {
            spinSpeedText.text = 0 + " / " + startSpinSpeed;
        }
    }


    void Start()
    {
        CheckPlayerType();
    }


    void Update()
    {
        
    }
}
