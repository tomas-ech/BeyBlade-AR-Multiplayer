using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerControl : MonoBehaviour
{
    public float spinSpeed = 3600f;
    public bool doSpin;
    public GameObject playerGraphics;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (doSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
        }
    }
}
