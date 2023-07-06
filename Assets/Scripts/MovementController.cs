using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    public float maxVelocityChange = 4f;
    public float tiltAmount = 10f;

    private Vector3 velocityVector = Vector3.zero;  //Velocidad inicial
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Toma el input del joystick
        float xMoveInput = joystick.Horizontal;
        float zMoveInput = joystick.Vertical;

        //Vectores de velocidad por eje 
        Vector3 horizontalMove = transform.right * xMoveInput;
        Vector3 verticalMove = transform.forward * zMoveInput;

        //Vector final de movimiento, aqui se agrega la velocidad
        Vector3 moveVelocityVector = -(horizontalMove + verticalMove).normalized * speed;

        //Aplicar el movimiento al objeto/jugador
        Move(moveVelocityVector);

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -joystick.Horizontal * speed * tiltAmount);
    }

    private void Move(Vector3 moveVelocityVector)
    {
        velocityVector = moveVelocityVector;
    }

    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            //Aplicar una fuerza igual a la cantidad de cambio en la vleocidad 
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(-velocityChange, ForceMode.Acceleration);
        }

        
    }
}
