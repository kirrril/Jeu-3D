using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class ChestMachine : TrainingMachineBase, IInteractable
{
    bool thisChestMachine;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_1";
    }

    protected override void Update()
    {
        base.Update();

        if (thisChestMachine)
        {
            Chest1TrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.chest1Training >= 0.35f)
        {
            if (IHM.instance.contextMessageCoroutName != "Chest1TrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(Chest1TrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "Chest1TrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    public IEnumerator Chest1TrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "CHEST 1 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "CHEST 1 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "CHEST 1 TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.chest1Training < 0.25f)
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
            thisChestMachine = true;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachineIsMoving", true);

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.8f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.8f, 1.2f);

            // CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            // CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            // observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);

            if (GameManager.instance.chest1Training <= 0.25f)
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
            thisChestMachine = false;

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("chestMachineIsMoving", false);

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

    protected override IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool("chestMachineIsMoving", true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool("chestMachineIsMoving", false);
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

    void Chest1TrainingProgress()
    {
        GameManager.instance.chest1Training += Time.deltaTime / 100;

        GameManager.instance.chest1Training = Mathf.Clamp(GameManager.instance.chest1Training, 0, 0.25f);
    }
}
