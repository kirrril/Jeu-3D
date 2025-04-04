using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

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

	private Coroutine fightCoroutine;

	private Transform attackingAgent;

	private float attackAngle = 20f;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

	public bool isMoving;

	public bool isTraining;

	public bool isGaming;

	public bool isClimbing;

	public bool isReadyToJump;

	public bool isChargingJump;

	public bool isJumping;

	public bool isLanded;

	public bool isReadyToAttack;

	public bool playerWasAttacked;

	public bool playerAttacks;

	public bool playerHasAttacked;

	public bool isFalling;

	public float chargeJump;

	public float playerSpeed;

	public bool lostLife;

	public bool isSubmissed;

	Coroutine fallCoroutine;

	Vector3 lastPosition;

	[SerializeField]
	AudioSource ambientSound;

	[SerializeField]
	public AudioSource voiceHa;

	[SerializeField]
	AudioSource voiceLeaveMeAlone;

	[SerializeField]
	AudioSource sfxLanding;

	[SerializeField]
	AudioSource sfxFalling;

	[SerializeField]
	public AudioSource sfxProteinCollect;

	[SerializeField]
	public AudioSource sfxWaterCollect;

	[SerializeField]
	Light spotLight;

	[SerializeField]
	Transform spotLightPosition;




	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		StartPosition();

		lastPosition = transform.position;

		ambientSound.Play();

		spotLight.enabled = false;
	}

	void Update()
	{
		GetInput();
		Jump();
		GroundControl();
		Climb();
	}

	void FixedUpdate()
	{
		CheckIfMoving();
		RotatePlayer();
		MovePlayer();

		if (!isClimbing)
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
		if (isTraining == false && isGaming == false && isClimbing == false)
		{
			float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

			Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}
	}


	void MovePlayer()
	{
		if (isTraining == false && isReadyToJump == false && isClimbing == false)
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
				voiceHa.Play();

				chargeJump += Time.deltaTime * 20;

				chargeJump = Mathf.Clamp(chargeJump, 0, 20);

				isChargingJump = true;
			}

			if (Input.GetKeyUp(KeyCode.Space))
			{
				ambientSound.Stop();

				rb.velocity = (transform.forward * chargeJump * 0.5f/* * GameManager.instance.currentPlayer.legsTraining*/) + (transform.up * chargeJump * 1.2f/* * GameManager.instance.currentPlayer.legsTraining*/);

				isChargingJump = false;

				isJumping = true;

				chargeJump = 0f;
			}
		}
	}


	public void Climb()
	{
		if (isClimbing)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
			}

			if (!Input.GetKey(KeyCode.Space))
			{
				if (transform.position.y > -8f)
				{
					transform.Translate(Vector3.down * 1.5f * Time.deltaTime);
				}

			}
		}
	}

	void GroundControl()
	{
		if (transform.position.y < -10.0f && fallCoroutine == null)
		{
			fallCoroutine = StartCoroutine(EnterTheVoid());
		}
	}


	IEnumerator EnterTheVoid()
	{
		IHM.instance.FadeToBlack();

		ambientSound.Stop();

		sfxFalling.Play();

		yield return new WaitForSeconds(2f);

		GameManager.instance.currentPlayer.life -= 1;

		isTraining = false;

		isMoving = true;

		isReadyToJump = false;

		StartPosition();

		yield return new WaitForSeconds(1f);

		IHM.instance.FadeOut();

		ambientSound.Play();

		sfxFalling.Stop();

		fallCoroutine = null;
	}


	IEnumerator Fight(GameObject communicator)
	{
		float elapsedTime = 0f;

		while (elapsedTime < 1f)
		{
			if (IsFacingAgent() && Input.GetKeyDown(KeyCode.Space))
			{
				Transform agentTransform = communicator.gameObject.transform.parent;

				playerAttacks = true;

				voiceLeaveMeAlone.Play();

				yield return new WaitForSeconds(0.2f);

				Transform enemyYeahGameObject = agentTransform.Find("Yeah");
				AudioSource enemyYeah = enemyYeahGameObject.GetComponentInChildren<AudioSource>();
				enemyYeah.volume = 0.5f;
				enemyYeah.Play();

				StartCoroutine(DefeatEnemy(communicator));
				fightCoroutine = null;

				enemyYeah.volume = 1f;

				yield break;
			}

			elapsedTime += Time.deltaTime;

			yield return null;
		}

		StartCoroutine(SufferSubmission(communicator));

		fightCoroutine = null;
	}


	bool IsFacingAgent()
	{
		if (attackingAgent == null) return false;

		Vector3 directionToAgent = (attackingAgent.position - transform.position).normalized;
		float angle = Vector3.Angle(transform.forward, directionToAgent);
		return angle <= attackAngle;
	}


	IEnumerator DefeatEnemy(GameObject communicator)
	{
		GameManager.instance.currentPlayer.defeatedEnemies += 1;

		spotLight.enabled = false;

		yield return null;

		Transform agentTransform = communicator.gameObject.transform.parent;
		Animator manAnimator = agentTransform.GetComponentInChildren<Animator>();
		manAnimator.SetBool("isSubmissed", true);

		IHM.instance.DisplayVictoryMessage();

		playerAttacks = false;

		playerHasAttacked = true;

		yield return new WaitForSeconds(3.5f);

		manAnimator.SetBool("isSubmissed", false);
	}


	public IEnumerator SufferSubmission(GameObject communicator)
	{
		yield return new WaitForSeconds(0.2f);

		playerWasAttacked = true;

		Transform agentTransform = communicator.gameObject.transform.parent;
		Animator manAnimator = agentTransform.GetComponentInChildren<Animator>();
		manAnimator.SetBool("isAttacking", true);

		yield return new WaitForSeconds(1.0f);

		Transform enemyYeahGameObject = agentTransform.Find("Yeah");
		AudioSource enemyYeah = enemyYeahGameObject.GetComponentInChildren<AudioSource>();
		enemyYeah.Play();

		isSubmissed = true;

		yield return new WaitForSeconds(0.7f);

		voiceHa.Play();

		ambientSound.Stop();

		lights.SetActive(false);

		starParticles.transform.position = koFocus.transform.position;
		starParticles.Play();

		yield return new WaitForSeconds(0.3f);

		manAnimator.SetBool("isAttacking", false);

		yield return new WaitForSeconds(1.5f);

		isSubmissed = false;

		yield return new WaitForSeconds(0.1f);

		GameManager.instance.currentPlayer.life -= 1;

		isTraining = false;

		isMoving = true;

		isReadyToJump = false;

		lostLife = false;

		lights.SetActive(true);

		StartPosition();

		ambientSound.Play();

		IHM.instance.DisplayDefeatMessage();

		playerWasAttacked = false;
		playerAttacks = false;
	}


	public void MakeHugs()
	{

	}



	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			ambientSound.Play();
		}

		if (other.CompareTag("Training"))
		{
			isTraining = true;

			isMoving = false;

			isReadyToJump = false;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(other.transform.position);
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = true;

			isTraining = false;

			isMoving = false;
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = true;

			isTraining = false;
		}

		if (other.CompareTag("Rope"))
		{
			isClimbing = true;

			isMoving = false;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(transform);
		}

		if (other.gameObject.name == "Communicator")
		{
			attackingAgent = other.gameObject.transform.parent;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(other.gameObject.transform.parent);

			if (GameManager.instance.currentPlayer.level == 4)
			{
				MakeHugs();
			}
			else if (GameManager.instance.currentPlayer.level == 3 || GameManager.instance.currentPlayer.level == 2 && GameManager.instance.currentPlayer.chestTraining >= 0.5f)
			{
				isReadyToAttack = true;

				fightCoroutine = StartCoroutine(Fight(other.gameObject));
			}
			else
			{
				StartCoroutine(SufferSubmission(other.gameObject));
			}
		}

		if (other.gameObject.name == "Level1")
		{
			GameManager.instance.currentPlayer.level = 1;
		}

		if (other.gameObject.name == "Level2")
		{
			GameManager.instance.currentPlayer.level = 2;

			IHM.instance.FlipLegCanvas();
		}

		if (other.gameObject.name == "Level3")
		{
			GameManager.instance.currentPlayer.level = 3;
		}


		if (other.CompareTag("YouWin"))
		{
			StartCoroutine(YouWin());
		}
	}

	IEnumerator YouWin()
	{
		GameManager.instance.YouWin();

		yield return null;

		Animator playerAnimator = GetComponentInChildren<Animator>();
		playerAnimator.SetBool("isClimbing", false);

		GetComponent<Rigidbody>().isKinematic = false;

		startPosition = GameObject.Find("StartPosition").transform;

		transform.position = startPosition.position;

		isClimbing = false;

		isMoving = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = false;

			isMoving = true;

			isReadyToJump = false;

			spotLight.enabled = false;
		}

		if (other.gameObject.name == "Communicator" && fightCoroutine != null)
		{
			StopCoroutine(fightCoroutine);
			fightCoroutine = null;
			attackingAgent = null;
			isReadyToAttack = false;
			playerAttacks = false;

			spotLight.enabled = false;
		}


		if (other.CompareTag("Rope"))
		{
			isClimbing = false;

			spotLight.enabled = false;
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

		sfxLanding.Play();

		yield return null;

		ambientSound.Play();

		yield return new WaitForSeconds(1.2f);

		isLanded = false;

		isJumping = false;

		isTraining = false;

		isMoving = false;

		isReadyToJump = false;
	}
}
