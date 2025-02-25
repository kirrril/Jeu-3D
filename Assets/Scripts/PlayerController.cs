using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[SerializeField]
	Transform startPosition;

	[SerializeField]
	private Rigidbody rb;

	private Vector3 inputMove = Vector3.zero;
	
	private float inputRotate = 0;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

	public bool isMoving;

	public bool isTraining;

	public bool isReadyToJump;

	public float chargeJump;

	public float playerSpeed;

	public bool isJumping;

	public bool isLanded;

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
		Jump();
	}

	void GetInput()
	{
		inputMove.x = Input.GetAxis("Horizontal");
		inputMove.y = Input.GetAxis("Vertical");
		inputRotate = Input.GetAxis("Mouse X");
	}

	void RotatePlayer()
	{
		if (isTraining == false)
		{
			float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

			Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}
	}


	void MovePlayer()
	{
		if (isTraining == false && isReadyToJump == false)
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
		float distanceMoved = (transform.position - lastPosition).magnitude;

		isMoving = distanceMoved > 0.01f;

		lastPosition = transform.position;
	}


	public void StartPosition()
	{
		transform.position = startPosition.position;
		transform.rotation = startPosition.rotation;
	}


	public void Jump()
	{
		if (isReadyToJump)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				chargeJump += Time.fixedDeltaTime * 20;

				chargeJump = Mathf.Clamp(chargeJump, 0, 20);
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				isJumping = true;

				rb.velocity = (transform.forward * chargeJump * 0.5f/* * GameManager.instance.currentPlayer.legsTraining*/) + (transform.up * chargeJump * 1.2f/* * GameManager.instance.currentPlayer.legsTraining*/);

				isReadyToJump = false;

				chargeJump = 0f;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = true;

			isMoving = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (isJumping)
			{
				isLanded = true;
			}

			isJumping = false;
		}

		if (collision.gameObject.CompareTag("DeadZone"))
		{
			GameManager.instance.currentPlayer.life -= 1;

			StartPosition();
		}
	}

}
