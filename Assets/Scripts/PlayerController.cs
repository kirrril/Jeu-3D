using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[SerializeField]
	Transform startPosition;

	[SerializeField]
	private Rigidbody rb;

	[SerializeField]
	private GameObject lights;

	[SerializeField]
	private Light directionalLight;

	[SerializeField]
	private ParticleSystem starParticles;

	[SerializeField]
	private Transform koFocus;

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

	public bool lostLife;

	public bool isSubmissed;

	public bool isDehydrated;



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
		// LifeManagement();
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
		isSubmissed = false;
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

	// public IEnumerator LoseLife()
	// {
	// 	isTraining = false;

	// 	isMoving = true;

	// 	isReadyToJump = false;

	// 	lostLife = false;

	// 	isSubmissed = false;

	// 	isDehydrated = false;

	// 	yield return new WaitForSeconds(10.0f);

	// 	GameManager.instance.currentPlayer.life -= 1;

	// 	StartPosition();

	// 	StopAllCoroutines();
	// }


	public IEnumerator SufferSubmission(GameObject communicator)
	{
		yield return new WaitForSeconds(1.0f);

		isSubmissed = true;

		yield return new WaitForSeconds(0.3f);

		lights.SetActive(false);

		starParticles.transform.position = koFocus.transform.position;
		starParticles.Play();


		yield return new WaitForSeconds(0.3f);

		Transform agentTransform = communicator.gameObject.transform.parent;
		Animator manAnimator = agentTransform.GetComponentInChildren<Animator>();
		manAnimator.SetBool("isAttacking", false);

		yield return new WaitForSeconds(1.5f);

		isSubmissed = false;

		yield return new WaitForSeconds(0.1f);

		GameManager.instance.currentPlayer.life -= 1;

		isTraining = false;

		isMoving = true;

		isReadyToJump = false;

		lostLife = false;

		isDehydrated = false;

		lights.SetActive(true);

		StartPosition();
	}


	// public void LifeManagement()
	// {
	// 	if (transform.position.y < -4f)
	// 	{
	// 		lostLife = true;
	// 		StartCoroutine(LoseLife());
	// 	}

	// 	if (isSubmissed)
	// 	{
	// 		StartCoroutine(LoseLife());
	// 	}

	// 	if (isDehydrated)
	// 	{
	// 		StartCoroutine(LoseLife());
	// 	}
	// }


	public void MakeHugs()
	{

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

		if (other.gameObject.name == "Communicator")
		{
			if (GameManager.instance.currentPlayer.level == 4)
			{
				MakeHugs();
			}
			else
			{
				Transform agentTransform = other.gameObject.transform.parent;
				Animator manAnimator = agentTransform.GetComponentInChildren<Animator>();
				manAnimator.SetBool("isAttacking", true);

				StartCoroutine(SufferSubmission(other.gameObject));
			}
		}

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
