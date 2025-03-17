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

	public bool isGaming;

	public bool isReadyToJump;

	public bool isChargingJump;

	public bool isJumping;

	public bool isLanded;

	public float chargeJump;

	public float playerSpeed;

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
		LoseLife();
		Jump();
	}

	void FixedUpdate()
	{
		CheckIfMoving();
		RotatePlayer();
		MovePlayer();


		rb.angularVelocity = Vector3.zero; // ????
	}

	void GetInput()
	{
		inputMove.x = Input.GetAxis("Horizontal");
		inputMove.y = Input.GetAxis("Vertical");
		inputRotate = Input.GetAxis("Mouse X");
	}

	void RotatePlayer()
	{
		if (isTraining == false && isGaming == false)
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
		if (isTraining == false && isReadyToJump == false)
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
		isReadyToJump = false;
		transform.position = startPosition.position;
		transform.rotation = startPosition.rotation;
	}


	public void Jump()
	{
		if (isReadyToJump)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				chargeJump += Time.deltaTime * 20;

				chargeJump = Mathf.Clamp(chargeJump, 0, 20);

				isChargingJump = true;
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				rb.velocity = (transform.forward * chargeJump * 0.5f/* * GameManager.instance.currentPlayer.legsTraining*/) + (transform.up * chargeJump * 1.2f/* * GameManager.instance.currentPlayer.legsTraining*/);

				isChargingJump = false;

				isJumping = true;

				chargeJump = 0f;
			}
		}
	}

	public void LoseLife()
	{
		if (transform.position.y < -8f)
		{
			GameManager.instance.currentPlayer.life -= 1;

			isTraining = false;

			isMoving = false;

			isReadyToJump = false;

			StartPosition();
		}
	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = true;

			isMoving = false;

			isReadyToJump = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = true;

			isTraining = false;

			isMoving = false;
		}

		// if (other.CompareTag("Door"))
		// {
		// 	isReadyToJump = false;

		// 	isTraining = false;

		// 	isMoving = true;

		// 	isGaming = false;
		// }

		if (other.CompareTag("Desk"))
		{
			isReadyToJump = false;

			isTraining = false;

			isMoving = false;

			isGaming = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = false;

			isMoving = false;

			isReadyToJump = false;
		}

		// if (other.CompareTag("Door"))
		// {
		// 	isReadyToJump = false;

		// 	isTraining = false;

		// 	isMoving = true;

		// 	isGaming = false;
		// }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (isJumping)
			{
				StartCoroutine(Landing());
			}
		}
	}

	IEnumerator Landing()
	{
		isLanded = true;

		yield return new WaitForSeconds(1.2f);

		isLanded = false;

		isJumping = false;

		isTraining = false;

		isMoving = false;

		isReadyToJump = false;
	}
}
