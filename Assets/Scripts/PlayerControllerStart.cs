using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStart : MonoBehaviour
{
    public static PlayerControllerStart instance;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    private Vector3 inputMove = Vector3.zero;

    private float inputRotate = 0;

    [SerializeField]
    private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

    public float playerSpeed;

    public bool isMoving = true;

    public bool isGaming;

    Vector3 lastPosition;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        StartPosition();

        lastPosition = transform.position;
    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        CheckIfMoving();
        RotatePlayer();
        MovePlayer();

        if (isGaming)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void GetInput()
    {
        inputMove.x = Input.GetAxis("Horizontal");
        inputMove.y = Input.GetAxis("Vertical");
        inputRotate = Input.GetAxis("Mouse X");
    }

    void RotatePlayer()
    {
        if (!isGaming)
        {
            float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

            Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }


    void MovePlayer()
    {
        if (!isGaming)
        {
            Vector3 forwardMove = transform.forward * inputMove.y * forwardSpeed;
            Vector3 sideMove = transform.right * inputMove.x * sideSpeed;

            Vector3 resultMove = forwardMove + sideMove;

            rb.MovePosition(transform.position + resultMove * Time.fixedDeltaTime);

            playerSpeed = rb.velocity.magnitude * 1000;
        }
    }


    void CheckIfMoving()
    {
        if (!isGaming)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                isMoving = true;

                animator.SetFloat("MovementSpeed", 2.1f);
            }
            else
            {
                isMoving = false;

                animator.SetFloat("MovementSpeed", 0.2f);
            }
        }
    }

    public void StartPosition()
    {
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Desk"))
        {
            isGaming = true;

            isMoving = false;
        }
    }
}
