using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class BackMachine1 : TrainingMachineBase, IInteractable
{
    bool thisBackMachine1;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPullingBackMachine1";

        machineAnimationBool = "backMachine1IsMoving";
    }

    protected override void Update()
    {
        base.Update();

        if (thisBackMachine1)
        {
            Back1TrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.back1Training >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "Back1TrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(Back1TrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "Back1TrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    public IEnumerator Back1TrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.back1Training < 0.167f)
        {
            float waterLoss = Time.deltaTime / 20;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            trainingAudio.Stop();

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            if (thirstyCoroutine == null)
            {
                thirstyCoroutine = StartCoroutine(ThirstyCorout());
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Man"))
        {
            return;
        }

        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            thisBackMachine1 = true;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1f, 0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(1f, 1.6f, -1.1f);

            if (GameManager.instance.back1Training <= 0.167f)
            {
                IHM.instance.DisplayWaterWarning();
            }
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Man"))
        {
            base.OnTriggerExit(other);
        }

        if (other.CompareTag("Player"))
        {
            thisBackMachine1 = false;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
        }
    }

    public override void Interact(GameObject user)
    {
        if (user.CompareTag("Girl"))
        {
            return;
        }
        else if (user.CompareTag("Man"))
        {
            AgentController controller = user.GetComponent<AgentController>();
            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

            if (!isInteractable)
            {
                controller.StartMoveToTarget();
            }

            if (user.CompareTag("Man"))
            {
                GameObject communicator = user.transform.Find("Communicator").gameObject;
                communicator.SetActive(false);
            }

            trainingPerson = user;

            controller.isBusy = true;

            agent.enabled = false;

            TakePlace(user);

            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            trainingCoroutine = StartCoroutine(TrainingCorout(user));
        }

        if (user.CompareTag("Player"))
        {
            if (!isInteractable)
            {
                return;
            }

            TakePlace(user);

            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            trainingAudio.Play();

            ambientSound.Stop();

            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

            Animator machineAnimator = GetComponentInChildren<Animator>();
            machineAnimator.SetBool(machineAnimationBool, true);

            PlayerController.instance.isTraining = true;

            StopTrainingButtonOn();
        }
    }

    protected override IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool(machineAnimationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool(machineAnimationBool, false);
        yield return new WaitForSeconds(0.1f);

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        if (user.CompareTag("Man"))
        {
            GameObject communicator = user.transform.Find("Communicator").gameObject;
            communicator.SetActive(true);
        }

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        controller.StartMoveToTarget();

        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }

    protected override void PlayerLeavePlace()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        PlayerController.instance.isTraining = false;

        GameObject player = GameObject.Find("Player");

        player.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool(machineAnimationBool, false);

        player.transform.position = stopTrainingPosition.position;
        player.transform.rotation = stopTrainingPosition.rotation;

        trainingAudio.Stop();
        ambientSound.Play();
    }

    void Back1TrainingProgress()
    {
        if (thisBackMachine1)
        {
            GameManager.instance.back1Training += Time.deltaTime / 100;

            GameManager.instance.back1Training = Mathf.Clamp(GameManager.instance.back1Training, 0, 0.167f);
        }
    }
}
