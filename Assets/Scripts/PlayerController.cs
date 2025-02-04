using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

    private Vector3 inputMove = Vector3.zero;
    private float inputRotate = 0;

    [SerializeField]
    private float forwardSpeed = 2, sideSpeed = 2, upSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;



    void Start()
    {

    }


    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    void GetInput()
    {
        inputMove.x = Input.GetAxis("Horizontal");
        inputMove.y = Input.GetAxis("Vertical");
        inputMove.z = Input.GetAxis("Jump");
        inputRotate = Input.GetAxis("Mouse X");
    }

    void RotatePlayer()
    {
        float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

        Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }


    void MovePlayer()
    {
        Vector3 forwardMove = transform.forward * inputMove.y * forwardSpeed;
        Vector3 sideMove = transform.right * inputMove.x * sideSpeed;
        Vector3 upMove = transform.up * inputMove.z * upSpeed;

        Vector3 resultMove = rb.rotation * forwardMove + sideMove + upMove;

        rb.velocity = resultMove;

    }


}
