using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

	[SerializeField]
	CameraSwitch cameraSwitch;

	[SerializeField]
	Transform cameraTarget;

	float cameraSensitivity = 2f;
	float minYPosition = -2f;
	float maxYPosition = 10f;
	float smoothFactor = 100f;

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

	private float inputCameraAngle = 0;

	private Coroutine fightCoroutine;

	private Transform attackingAgent;

	private float attackAngle = 20f;

	[SerializeField]
	private float forwardSpeed = 2, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

	public bool isMoving;

	public bool isTraining;

	public bool isGaming;

	public bool isClimbing;

	public bool isClimbingUp;

	// public bool isSlidingDown;

	public bool isInClimbingZone;

	[SerializeField]
	GameObject climbingPosition;

	bool isInitializedAtClimbingPosition;

	[SerializeField]
	GameObject stopClimbingPosition;

	private float climbSpeed = 1.5f;
	private float climbGroundY = -4.5f;

	bool maxwHeightReached;

	bool isOnTheFloor = false;

	bool canWalk = true;


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
	}

	void Update()
	{
		GetInput();

		if (isTraining == false && isGaming == false && isClimbing == false && playerWasAttacked == false)
		{
			LiftCamera();
		}

		CheckIfMoving();

		if (isInJumpZone)
		{
			Jump();
		}

		if (isInClimbingZone)
		{
			Climb();
		}

		GroundControl();
	}

	void FixedUpdate()
	{
		// CheckIfMovingInJumpingZone();

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
		inputCameraAngle = Input.GetAxis("Mouse Y");
	}

	void RotatePlayer()
	{
		if (isTraining == false && isGaming == false && isClimbing == false && playerWasAttacked == false)
		{
			float playerRotation = inputRotate * rotationCoeff * rotationSpeed * Time.fixedDeltaTime;

			Quaternion deltaRotation = Quaternion.Euler(0, playerRotation, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}
	}


	void LiftCamera()
	{
		Vector3 localPosition = cameraTarget.localPosition;
		float targetY = localPosition.y + inputCameraAngle * cameraSensitivity * Time.deltaTime;

		targetY = Mathf.Clamp(targetY, minYPosition, maxYPosition);

		localPosition.y = Mathf.Lerp(localPosition.y, targetY, smoothFactor * Time.deltaTime);

		cameraTarget.localPosition = localPosition;
	}


	void MovePlayer()
	{
		if (canWalk)
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
		if (/*!isTraining && !isReadyToJump && !isClimbing && !isInClimbingZone && !isInJumpZone || */canWalk)
		{
			if (Mathf.Abs(inputMove.x) > 0.1f || Mathf.Abs(inputMove.y) > 0.1f)
			{
				isMoving = true;
			}
			else
			{
				isMoving = false;
			}
		}
		else
		{
			isMoving = false;
		}
	}

	public void StartPosition()
	{
		isSubmissed = false;
		isTraining = false;
		isReadyToJump = false;
		transform.position = startPosition.position;
		transform.rotation = startPosition.rotation;

		Transform cameraTarget = GameObject.Find("CameraTarget").transform;
		cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

		CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
		CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
		playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
	}


	// IEnumerator SetJumpZone()
	// {
	// 	yield return new WaitForSeconds(1f);

	// 	isInJumpZone = true;
	// }

	public void Jump()
	{
		// if (isInJumpZone)
		// {
		if (isReadyToJump && Input.GetKey(KeyCode.Space))
		{
			voiceHa.Play();

			chargeJump += Time.deltaTime * 20;

			chargeJump = Mathf.Clamp(chargeJump, 0, 20);

			isChargingJump = true;
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (GameManager.instance.currentPlayer.legsTraining < 1f)
			{
				if (GameManager.instance.currentPlayer.legsTraining < 0.5f)
				{
					jumpingCoeff = 5f;
				}
				else if (GameManager.instance.currentPlayer.legsTraining > 0.5f && GameManager.instance.currentPlayer.legsTraining < 1f)
				{
					jumpingCoeff = 10f;
				}

				rb.velocity = transform.up * jumpingCoeff;

				ambientSound.Stop();

				isReadyToJump = false;

				isChargingJump = false;

				isJumping = true;

				chargeJump = 0f;

				NoLandEnabled(true);
			}
			else
			{
				rb.velocity = (transform.forward * chargeJump * 0.5f * GameManager.instance.currentPlayer.legsTraining) + (transform.up * chargeJump * 1.2f * GameManager.instance.currentPlayer.legsTraining);

				ambientSound.Stop();

				isReadyToJump = false;

				isChargingJump = false;

				isJumping = true;

				chargeJump = 0f;

				NoLandEnabled(true);
			}
		}
		// }
	}


	void NoLandEnabled(bool enabled)
	{
		foreach (GameObject colliderObject in noLandColliderObjects)
		{
			colliderObject.SetActive(enabled);
		}
	}

	IEnumerator CanWalkCorout()
	{
		yield return new WaitForSeconds(1f);

		canWalk = true;
	}

	public void Climb()
	{
		float maxHeight = GetMaxClimbHeight();

		spotLight.transform.LookAt(transform.position);
		spotLight.transform.position = spotLightPosition.position;

		if (Input.GetKey(KeyCode.Space))
		{
			isClimbing = true;
			rb.isKinematic = true;
			isOnTheFloor = false;
			canWalk = false;

			climbSpeed = 2f;

			if (!isInitializedAtClimbingPosition)
			{
				transform.position = climbingPosition.transform.position;
				transform.rotation = climbingPosition.transform.rotation;

				isInitializedAtClimbingPosition = true;
			}

			if (transform.position.y < maxHeight && !maxwHeightReached)
			{
				isClimbingUp = true;
				transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
			}
			else
			{
				maxwHeightReached = true;

				if (transform.position.y > climbGroundY)
				{
					isClimbingUp = false;
					transform.Translate(Vector3.down * climbSpeed * Time.deltaTime);
				}
				else
				{
					GetReadyToClimb();
				}
			}
		}
		else
		{
			maxwHeightReached = false;

			if (Input.GetKeyUp(KeyCode.Space))
			{
				climbSpeed = 4f;
			}

			if (transform.position.y > climbGroundY)
			{
				isClimbing = true;
				isClimbingUp = false;
				transform.Translate(Vector3.down * climbSpeed * Time.deltaTime);
			}
			else
			{
				GetReadyToClimb();
			}
		}
	}

	float GetMaxClimbHeight()
	{
		float backTraining = GameManager.instance.currentPlayer.backTraining;
		if (backTraining < 0.2f) return -2.5f;
		if (backTraining < 0.5f) return 3f;
		if (backTraining < 0.8f) return 4.5f;
		return float.MaxValue;
	}

	void GetReadyToClimb()
	{
		isClimbing = false;
		isClimbingUp = false;
		// isSlidingDown = false;
		rb.isKinematic = false;

		if (!isOnTheFloor)
		{
			transform.position = stopClimbingPosition.transform.position;
			transform.rotation = stopClimbingPosition.transform.rotation;
			isOnTheFloor = true;
		}

		maxwHeightReached = false;

		isInitializedAtClimbingPosition = false;

		canWalk = true;
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

		canWalk = false;

		// float rotationDuration = 0.3f;
		// float elapsedTime = 0f;
		// while (elapsedTime < rotationDuration)
		// {
		// 	LookAtEnemy(transform, agentTransform, 3f);
		// 	LookAtEnemy(agentTransform, transform, 3f);
		// 	elapsedTime += Time.deltaTime;
		// 	yield return null;
		// }
		Transform defeatPoint = communicator.gameObject.transform.parent.Find("DefeatPoint");

		transform.position = defeatPoint.position;
		transform.rotation = defeatPoint.rotation;

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

		// isMoving = true;

		// isReadyToJump = false;

		lostLife = false;

		lights.SetActive(true);

		StartPosition();

		ambientSound.Play();

		IHM.instance.DisplayDefeatMessage();

		playerWasAttacked = false;
		playerAttacks = false;
		canWalk = true;
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

			canWalk = false;

			// isMoving = false;

			// isReadyToJump = false;

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(other.transform.position);
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = true;

			// isTraining = false;

			isInJumpZone = true;

			canWalk = false;

			StartCoroutine(CanWalkCorout());
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = true;

			// isTraining = false;

			isInJumpZone = true;

			canWalk = true;
		}

		if (other.CompareTag("Rope"))
		{
			spotLight.enabled = true;

			cameraSwitch.playerCam.Priority = 0;
			cameraSwitch.observerCam.Priority = 10;

			isInClimbingZone = true;

			canWalk = true;
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

		// Animator playerAnimator = GetComponentInChildren<Animator>();
		// playerAnimator.SetBool("isClimbing", false);

		GetComponent<Rigidbody>().isKinematic = false;

		startPosition = GameObject.Find("StartPosition").transform;

		transform.position = startPosition.position;

		isClimbingUp = false;
		isClimbing = false;

		// isMoving = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = false;

			canWalk = true;

			// isMoving = true;

			// isReadyToJump = false;

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
			isInClimbingZone = false;

			spotLight.enabled = false;

			cameraSwitch.playerCam.Priority = 10;
			cameraSwitch.observerCam.Priority = 0;

			// canWalk = false;
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = false;

			isInJumpZone = false;

			// canWalk = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = false;

			isInJumpZone = false;

			// canWalk = false;
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
			StartCoroutine(LandingOnTrampoline());
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

		isReadyToJump = false;

		isInJumpZone = false;

		NoLandEnabled(false);
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
}
