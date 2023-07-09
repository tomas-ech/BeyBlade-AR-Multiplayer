using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SynchronizationScript : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkPosition;
    Quaternion networkRotation;

    public bool syncVelocity = true;
    public bool syncAngularVelocity = true;
    public bool isTeleportEnabled = true;

    public float teleportIfDistanceisGreaterThan = 1f;
    private float distance;
    private float angle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkPosition = new Vector3();
        networkRotation = new Quaternion();
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, distance * (1.0f / PhotonNetwork.SerializationRate));

            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Si esta escribiendo el photonview es MIO y este es mi pj
            //enviamos posicion, velocidad, etc a los demas jugadores
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if (syncVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if (syncAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }


        }
        else
        {
            //En jugadores remotos
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if (Vector3.Distance(rb.position, networkPosition) > teleportIfDistanceisGreaterThan)
                {
                    rb.position = networkPosition;
                }
            }

            if (syncVelocity || syncAngularVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if (syncVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkPosition += rb.velocity * lag;

                    distance = Vector3.Distance(rb.position, networkPosition);
                }

                if (syncAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkRotation;

                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }
        }
    }
}
