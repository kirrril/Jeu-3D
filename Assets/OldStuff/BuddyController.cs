using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuddyController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Camera cam;

    private float currentVelocity;
    private float smoothTime = 0.05f;

    private Vector2 input = Vector2.zero;

    [SerializeField]
    private float forwardSpeed = 1, sideSpeed = 1;


    private float moveSpeed = 3.0f;

    void Start()
    {
        
    }


    void Update()
    {
       ReadInput();

    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void ReadInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    void MovePlayer()
    {
        Vector3 forwardDir = transform.forward * input.y;
        forwardDir *= forwardSpeed;

        Vector3 sideDir = Vector3.Cross(Vector3.up, transform.forward) * input.x;
        sideDir *= sideSpeed;

        Vector3 finalDir = forwardDir + sideDir;

        rb.MovePosition(transform.position + (finalDir * Time.fixedDeltaTime));

        float targetRotation = cam.transform.eulerAngles.y;
        float playerAngleDamp = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, playerAngleDamp, 0);
    }
}
