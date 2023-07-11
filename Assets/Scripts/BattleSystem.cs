using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleSystem : MonoBehaviourPun
{
    public SpinnerControl spinnerScript;

    private Rigidbody rb;

    private float startSpinSpeed;
    private float currentSpinSpeed;
    public GameObject uI_3dGo;
    public GameObject uI_DeathPrefab;
    private GameObject uI_DeathPanelGo;
    public Image spinSpeedBar;
    public TextMeshProUGUI spinSpeedText;

    public float damageCoefficient = 0.04f;
    public bool isAttacker;
    public bool isDefender;
    private bool isDead;

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
        if (!isDead)
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

            if (currentSpinSpeed < 100)
            {
                //die
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;

        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.spinSpeed = 0f;

        uI_3dGo.SetActive(false);

        if (photonView.IsMine)
        {
            //conteo para el respawn
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        GameObject  canvasGo = GameObject.Find("Canvas");

        if (uI_DeathPanelGo == null)
        {
            uI_DeathPanelGo = Instantiate(uI_DeathPrefab, canvasGo.transform);
        }
        else
        {
            uI_DeathPanelGo.SetActive(true);
        }

        Text respawnTimeText = uI_DeathPanelGo.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while (respawnTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            respawnTime -= 1f;
            respawnTimeText.text = respawnTime.ToString(".00");

            GetComponent<MovementController>().enabled = false;
        }

        uI_DeathPanelGo.SetActive(true);

        GetComponent<MovementController>().enabled = true;

        photonView.RPC("Reborn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void Reborn()
    {
        spinnerScript.spinSpeed = startSpinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedText.text = currentSpinSpeed + " / " + startSpinSpeed;

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        uI_3dGo.SetActive(true);

        isDead = false;
    }



    void Start()
    {
        CheckPlayerType();

        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
    }
}
