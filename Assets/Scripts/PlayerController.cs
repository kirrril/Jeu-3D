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

	[SerializeField]
	Animator animator;

	private Vector3 inputMove = Vector3.zero;
	private float inputRotate = 0;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;


	public static bool isTraining;

	public static bool isReadyToJump;

	public bool isGrounded;

	private float chargeJump;


	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		StartPosition();
	}

	void Update()
	{
		GetInput();

		Jump();
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

			rb.MovePosition(transform.position + resultMove * Time.deltaTime);
		}
	}

	public void StartPosition()
	{
		transform.position = startPosition.position;
		transform.rotation = startPosition.rotation;
	}

	void Jump()
	{
		if (isGrounded && isReadyToJump)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				chargeJump += Time.deltaTime * 20;

				chargeJump = Mathf.Clamp(chargeJump, 0, 20);
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				rb.velocity = (transform.forward * chargeJump * 0.5f/* * GameManager.instance.currentPlayer.legsTraining*/) + (transform.up * chargeJump * 1.2f/* * GameManager.instance.currentPlayer.legsTraining*/);
				IHM.instance.stopTrainingButton.gameObject.SetActive(false);
				isGrounded = false;
				chargeJump = 0f;
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isReadyToJump = false;

			isGrounded = true;

			Debug.Log("Touching ground");

			animator.SetTrigger("Landing");
		}
	}
}
