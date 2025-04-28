using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class ChestMachine2 : TrainingMachineBase, IInteractable
{
    bool thisChestMachine2;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_2";
    }

    protected override void Update()
    {
        base.Update();

        if (thisChestMachine2)
        {
            Chest2TrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.chest2Training >= 0.25f)
        {
            if (IHM.instance.contextMessageCoroutName != "Chest2TrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(Chest2TrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "Chest2TrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    public IEnumerator Chest2TrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "CHEST 2 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "CHEST 2 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "CHEST 2 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.chest2Training < 0.25f)
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

            // ambientSound.Play();

            // GameManager.instance.currentPlayer.life -= 1;

            // GameManager.instance.currentPlayer.water = 0.5f;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            thisChestMachine2 = true;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.8f, 1.8f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);

            if (GameManager.instance.chest2Training <= 0.25f)
            {
                IHM.instance.DisplayWaterWarning();
            }
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
            thisChestMachine2 = false;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", false);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(4f, 4.5f, -2f);
        }
    }

    public override void Interact(GameObject user)
    {
        if (!user.CompareTag("Man") && !user.CompareTag("Player"))
        {
            return;
        }

        if (user.CompareTag("Man"))
        {

            AgentController controller = user.GetComponent<AgentController>();
            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

            // if (controller.currentCoroutine != null)
            // {
            //     StopCoroutine(controller.currentCoroutine);
            //     controller.currentCoroutine = null;
            //     controller.currentCoroutineName = "null";
            // }

            if (!isInteractable)
            {
                controller.StartMoveToTarget();
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
            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachine2IsMoving", true);

            PlayerController.instance.isTraining = true;

            StopTrainingButtonOn();
        }
    }

    protected override IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool("chestMachine2IsMoving", true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool("chestMachine2IsMoving", false);
        yield return new WaitForSeconds(0.1f);

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        controller.StartMoveToTarget();

        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }

    void Chest2TrainingProgress()
    {
        GameManager.instance.chest2Training += Time.deltaTime / 100;

        GameManager.instance.chest2Training = Mathf.Clamp(GameManager.instance.chest2Training, 0, 0.25f);
    }
}
