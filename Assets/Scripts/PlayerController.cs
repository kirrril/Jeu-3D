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
	GameObject soundCtrlPanel;

	[SerializeField]
	CinemachineVirtualCamera playerCam;

	[SerializeField]
	Transform cameraTarget;

	[SerializeField]
	Transform cameraPlace;

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
	private float forwardSpeed = 2, forwardSpeedCoeff = 1.5f, sideSpeed = 2, rotationSpeed = 5, rotationCoeff = 100;

	public bool isMoving;

	public bool isTraining;

	public bool isGaming;

	public bool isClimbing;

	public bool isClimbingUp;

	public bool isInClimbingZone;



	[SerializeField]
	GameObject climbingPosition;

	bool isInitializedAtClimbingPosition;

	[SerializeField]
	GameObject stopClimbingPosition;

	private float climbSpeed = 1.5f;
	private float climbGroundY = -7.5f;

	bool maxwHeightReached;

	bool isOnTheFloor = false;


	public bool isPushingTheDoor;


	public bool canWalk = true;


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

	[SerializeField]
	SpawnerPlates spawnerPlates;

	Coroutine hitByWeightPlateCorout;

	[SerializeField]
	GameObject head;



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

		head.SetActive(false);
	}

	void Update()
	{
		if (isInJumpZone)
		{
			Jump();
		}

		GroundControl();
	}

	void FixedUpdate()
	{


		if (isInClimbingZone)
		{
			Climb();
		}

		GetInput();
		CheckIfMoving();
		RotatePlayer();
		MovePlayer();

		if (!isTraining && !isInClimbingZone && !playerWasAttacked)
		{
			LiftCamera();
		}

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
		float targetY = localPosition.y + inputCameraAngle * cameraSensitivity * Time.fixedDeltaTime;

		targetY = Mathf.Clamp(targetY, minYPosition, maxYPosition);

		localPosition.y = Mathf.Lerp(localPosition.y, targetY, smoothFactor * Time.fixedDeltaTime);

		cameraTarget.localPosition = localPosition;
	}


	void MovePlayer()
	{
		if (canWalk)
		{
			Vector3 forwardMove = transform.forward * inputMove.y * forwardSpeed;
			Vector3 sideMove = transform.right * inputMove.x * sideSpeed;

			Vector3 resultMove = forwardMove + sideMove;

			rb.MovePosition(transform.position + resultMove * Time.fixedDeltaTime * forwardSpeedCoeff);
		}
	}


	void CheckIfMoving()
	{
		if (canWalk)
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

		cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

		playerCam.Follow = transform;

		CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
		playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
	}


	public void Jump()
	{
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

		if (!isOnTheFloor)
		{
			rb.useGravity = false;

			rb.MovePosition(climbingPosition.transform.position);
			rb.MoveRotation(climbingPosition.transform.rotation);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			canWalk = false;
			isOnTheFloor = false;

			isClimbing = true;
			isClimbingUp = true;

			if (transform.position.y >= maxHeight)
			{
				maxwHeightReached = true;
			}

			if (maxwHeightReached)
			{
				climbSpeed = 0.01f;

				climbingPosition.transform.position -= Vector3.up * climbSpeed;

				isClimbing = true;
				isClimbingUp = false;
			}
			else
			{
				climbSpeed = 0.015f;

				climbingPosition.transform.position += Vector3.up * climbSpeed;

				isClimbing = true;
				isClimbingUp = true;
			}
		}
		else
		{
			isClimbing = true;
			isClimbingUp = false;

			maxwHeightReached = false;

			climbSpeed = 0.03f;

			if (transform.position.y > climbGroundY)
			{
				climbingPosition.transform.position -= Vector3.up * climbSpeed;
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
		if (backTraining < 0.5f) return -2.5f;
		if (backTraining < 0.8f) return -2.5f;
		return float.MaxValue;
	}

	void GetReadyToClimb()
	{
		isInClimbingZone = true;

		if (!isOnTheFloor)
		{
			rb.MovePosition(stopClimbingPosition.transform.position);
			rb.MoveRotation(stopClimbingPosition.transform.rotation);

			canWalk = false;
			StartCoroutine(CanWalkCorout());
			isOnTheFloor = true;
		}

		isClimbing = false;
		isClimbingUp = false;

		maxwHeightReached = false;

		// canWalk = true;

		rb.useGravity = true;
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

		if (isTraining || isPushingTheDoor || playerWasAttacked)
		{
			yield break;
		}

		playerWasAttacked = true;

		Transform agentTransform = communicator.gameObject.transform.parent;

		canWalk = false;

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


	public void DeadHitByWeightPlate()
	{
		hitByWeightPlateCorout = StartCoroutine(HitByWeightPlate());
	}


	IEnumerator HitByWeightPlate()
	{
		cameraTarget.localPosition = Vector3.zero;

		playerCam.Follow = cameraPlace.transform;

		yield return null;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = true;

			canWalk = false;

			soundCtrlPanel.SetActive(true);

			spotLight.enabled = true;
			spotLight.transform.position = spotLightPosition.position;
			spotLight.transform.LookAt(other.transform.position);
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = true;

			isInJumpZone = true;

			canWalk = false;

			StartCoroutine(CanWalkCorout());
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = true;

			isInJumpZone = true;

			canWalk = true;
		}

		if (other.CompareTag("Pole"))
		{
			spotLight.enabled = true;

			GetReadyToClimb();
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
		}

		if (other.gameObject.name == "Level3")
		{
			GameManager.instance.currentPlayer.level = 3;

			head.SetActive(true);

			spawnerPlates.SpawnWeightPlates();
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

		startPosition = GameObject.Find("StartPosition").transform;

		transform.position = startPosition.position;

		isClimbingUp = false;
		isClimbing = false;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Training"))
		{
			isTraining = false;

			canWalk = true;

			soundCtrlPanel.SetActive(false);

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


		if (other.CompareTag("Pole"))
		{
			isInClimbingZone = false;

			isOnTheFloor = false;

			spotLight.enabled = false;
		}

		if (other.CompareTag("JumpZone"))
		{
			isReadyToJump = false;

			isInJumpZone = false;
		}

		if (other.CompareTag("Trampoline"))
		{
			isReadyToJump = false;

			isInJumpZone = false;
		}

		if (other.gameObject.name == "Level3")
		{
			GameManager.instance.currentPlayer.level = 0;

			spawnerPlates.StopSpawningWeightPlates();
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
