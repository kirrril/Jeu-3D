using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody rb;

	private Vector3 inputMove = Vector3.zero;
	private float inputRotate = 0;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;


	private Vector3 trainingPosition;
	private Vector3 exitPosition;

	public static bool isTrainig = false;


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

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Treadmill"))
		{
			isTrainig = true;
			trainingPosition = new Vector3(0, 0.27f, 0);
		}

		transform.position = other.transform.position + trainingPosition;
		transform.rotation = other.transform.rotation;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Treadmill"))
		{
			if (Input.GetKeyDown(KeyCode.Keypad0))
			{
				transform.position = other.transform.position + new Vector3(0, -0.07f, 1f);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		isTrainig = false;
	}

}
