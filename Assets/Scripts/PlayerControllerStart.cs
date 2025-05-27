using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerStart : MonoBehaviour
{
    public static PlayerControllerStart instance;

    [SerializeField]
    Transform startPosition;

    [SerializeField]
    Transform cameraPlace;

    [SerializeField]
    Transform cameraTarget;
    float cameraSensitivity = 2f;
    float minYPosition = -2f;
    float maxYPosition = 10f;
    float smoothFactor = 100f;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Animator animator;

    private Vector3 inputMove = Vector3.zero;

    private float inputRotate = 0;

    private float forwardSpeed = 2f, forwardSpeedCoeff = 1.7f, sideSpeed = 2f, rotationSpeed = 5f, rotationCoeff = 100f;

    public float playerSpeed;

    private float inputCameraAngle = 0;

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


    void FixedUpdate()
    {
        GetInput();

        if (!isGaming)
        {
            MovePlayer();
            RotatePlayer();
            LiftCamera();
        }

        if (isGaming)
        {
            rb.angularVelocity = Vector3.zero;
        }

        CheckIfMoving();
    }

    void GetInput()
    {
        inputMove.x = Input.GetAxis("Horizontal");
        inputMove.y = Input.GetAxis("Vertical");
        inputRotate = Input.GetAxis("Mouse X");
        inputCameraAngle = Input.GetAxis("Mouse Y");
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

            rb.MovePosition(transform.position + resultMove * Time.fixedDeltaTime * forwardSpeedCoeff);
        }
    }


    void CheckIfMoving()
    {
        if (!isGaming)
        {
            if (Mathf.Abs(inputMove.x) > 0.1f || Mathf.Abs(inputMove.y) > 0.1f)
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
        else
        {
            isMoving = false;

            animator.SetFloat("MovementSpeed", 0.2f);
        }
    }

    void LiftCamera()
    {
        if (!isGaming)
        {
            Vector3 localPosition = cameraTarget.localPosition;
            float targetY = localPosition.y + inputCameraAngle * cameraSensitivity * Time.fixedDeltaTime;

            targetY = Mathf.Clamp(targetY, minYPosition, maxYPosition);

            localPosition.y = Mathf.Lerp(localPosition.y, targetY, smoothFactor * Time.fixedDeltaTime);

            cameraTarget.localPosition = localPosition;
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
