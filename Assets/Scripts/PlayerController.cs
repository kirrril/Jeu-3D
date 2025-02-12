using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[SerializeField]
	private Rigidbody rb;

	[SerializeField]
	Animator animator;

	private Vector3 inputMove = Vector3.zero;
	private float inputRotate = 0;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;


	private Vector3 trainingPosition;
	private Vector3 exitPosition;

	public static bool isTrainig = false;


	void Awake()
	{
		instance = this;
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
		inputRotate = Input.GetAxis("Mouse X");
	}

	void RotatePlayer()
	{
		if (isTrainig == false)
		{
			float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

			Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}
	}


	void MovePlayer()
	{
		if (isTrainig == false)
		{
			Vector3 forwardMove = transform.forward * inputMove.y * forwardSpeed;
			Vector3 sideMove = transform.right * inputMove.x * sideSpeed;

			Vector3 resultMove = forwardMove + sideMove;

			rb.MovePosition(transform.position + resultMove * Time.deltaTime);
		}
	}
}
