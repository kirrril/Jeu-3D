using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerYouLose : MonoBehaviour
{
    public static PlayerControllerYouLose instance;

	[SerializeField]
	Transform startPosition;

	[SerializeField]
	private Rigidbody rb;

	private Vector3 inputMove = Vector3.zero;

	private float inputRotate = 0;


	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

	public bool isMoving;

	public bool isGaming;

	public float playerSpeed;

	Vector3 lastPosition;


    void Awake()
    {
        instance = this;

		// Destroy(GameManager.instance);
		// Destroy(PersistantData.instance);
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
	}

	void GetInput()
	{
		inputMove.x = Input.GetAxis("Horizontal");
		inputMove.y = Input.GetAxis("Vertical");
		inputRotate = Input.GetAxis("Mouse X");
	}

	void RotatePlayer()
	{
		if (isGaming == false)
		{
			float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

			Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}
	}


	void MovePlayer()
	{
		if (isGaming == false)
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
		if (isGaming == false)
		{
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
			{
				isMoving = true;
			}
			else
			{
				isMoving = false;
			}
		}

	}
	public void StartPosition()
	{
		transform.position = startPosition.position;
		transform.rotation = startPosition.rotation;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Desk"))
		{
			isMoving = false;

			isGaming = true;

			transform.position = new Vector3(0f, 0f, 3.33f);
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}
}
