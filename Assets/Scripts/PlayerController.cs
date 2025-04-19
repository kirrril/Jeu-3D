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

	float jumpingCoeff;

	public bool isLanding;

	public bool isInJumpZone;

	public bool hasFallen;

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

	[SerializeField]
	List<GameObject> noLandColliderObjects;

	private bool isLandingCoroutineRunning;

	// GameObject trampoline;




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

		NoLandEnabled(false);

		// trampoline = GameObject.Find("Trampoline");
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
		CheckIfMovingInJumpingZone();
		CheckIfMoving();
		RotatePlayer();
		MovePlayer();
		MovePlayerInJumpZone();

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


	void MovePlayerInJumpZone()
	{
		if (isInJumpZone)
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

	void CheckIfMovingInJumpingZone()
	{
		if (isInJumpZone == true)
		{
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
			{
				isReadyToJump = false;
				isMoving = true;
			}
			else
			{
				isReadyToJump = true;
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


	IEnumerator SetJumpZone()
	{
		yield return new WaitForSeconds(1f);

		isInJumpZone = true;
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

				isReadyToJump = false;

				isChargingJump = false;

				isJumping = true;

				chargeJump = 0f;

				NoLandEnabled(true);

				if (GameManager.instance.currentPlayer.legsTraining < 0.5f)
				{
					jumpingCoeff = 5f;
				}
				else if (GameManager.instance.currentPlayer.legsTraining > 0.5f && GameManager.instance.currentPlayer.legsTraining < 1f)
				{
					jumpingCoeff = 10f;
				}

				rb.velocity = transform.up * jumpingCoeff;

				if (GameManager.instance.currentPlayer.legsTraining >= 1f)
				{
					rb.velocity = (transform.forward * chargeJump * 0.5f * GameManager.instance.currentPlayer.legsTraining) + (transform.up * chargeJump * 1.2f * GameManager.instance.currentPlayer.legsTraining);
				}
			}
		}
	}


	IEnumerator LandingOnTrampoline()
	{
		isJumping = false;

		isLanding = true;

		sfxLanding.Play();

		yield return null;

		ambientSound.Play();

		yield return null;

		isLanding = false;

		isTraining = false;

		NoLandEnabled(false);
	}


	void NoLandEnabled(bool enabled)
	{
		foreach (GameObject colliderObject in noLandColliderObjects)
		{
			colliderObject.SetActive(enabled);
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

			spotLight.transform.position = spotLightPosition.position;
		}
	}

	void GroundControl()
	{
		if (transform.position.y < -10.0f && fallCoroutine == null)
		{
			isJumping = false;

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

		isJumping = false;

		isReadyToJump = false;

		hasFallen = true;

		NoLandEnabled(false);

		StartPosition();

		yield return new WaitForSeconds(1f);

		IHM.instance.FadeOut();

		ambientSound.Play();

		sfxFalling.Stop();

		fallCoroutine = null;

		hasFallen = false;
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
		yield return new WaitForSeconds(1f);

		if (isTraining)
		{
			yield break;
		}

		if (playerWasAttacked)
		{
			yield break;
		}

		playerWasAttacked = true;

		Transform agentTransform = communicator.gameObject.transform.parent;

		float rotationDuration = 0.5f;
		float elapsedTime = 0f;
		while (elapsedTime < rotationDuration)
		{
			LookAtEnemy(transform, agentTransform, 3f);
			LookAtEnemy(agentTransform, transform, 3f);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

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

	void LookAtEnemy(Transform target, Transform self, float speed)
	{
		Vector3 direction = target.position - self.position;
		if (direction != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(direction);
			self.rotation = Quaternion.Slerp(self.rotation, targetRotation, speed * Time.deltaTime);
		}
	}


	private void OnTriggerEnter(Collider other)
	{
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

			StartCoroutine(SetJumpZone());
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = true;

			isTraining = false;

			isInJumpZone = true;
		}

		if (other.CompareTag("Rope"))
		{
			isClimbing = true;

			isMoving = false;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(transform.position);
		}

		if (other.gameObject.name == "Communicator")
		{
			attackingAgent = other.gameObject.transform.parent;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(other.gameObject.transform.parent);

			if (GameManager.instance.currentPlayer.level == 3 || GameManager.instance.currentPlayer.level == 2 && GameManager.instance.currentPlayer.chestTraining >= 0.5f)
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

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = false;

			isInJumpZone = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isInJumpZone = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			if (isJumping && !isLandingCoroutineRunning)
			{
				StartCoroutine(Landing(1.2f));
			}
		}

		if (collision.gameObject.CompareTag("TrampolineLanding"))
		{
			// Debug.Log("Collision detected");

			// if (isJumping && !isLandingCoroutineRunning)
			// {
			StartCoroutine(LandingOnTrampoline());
			// }
		}
	}

	IEnumerator Landing(float delay)
	{
		isLandingCoroutineRunning = true;

		isJumping = false;

		isLanding = true;

		sfxLanding.Play();

		yield return null;

		ambientSound.Play();

		yield return new WaitForSeconds(delay);

		isLanding = false;

		isLandingCoroutineRunning = false;

		isTraining = false;

		NoLandEnabled(false);
	}



}
