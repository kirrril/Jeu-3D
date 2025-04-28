using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class BackMachine2 : TrainingMachineBase, IInteractable
{
    bool thisBackMachine2;

    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPullingBackMachine2";
    }

    protected override void Update()
    {
        base.Update();

        if (thisBackMachine2)
        {
            Back2TrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.back2Training >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "Back2TrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(Back2TrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "Back2TrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    public IEnumerator Back2TrainingCompletedWarning()
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
        if (GameManager.instance.back2Training < 0.167f)
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
            thisBackMachine2 = true;

            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.5f, 1.5f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);

            if (GameManager.instance.back2Training <= 0.167f)
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
            thisBackMachine2 = false;

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
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("backMachine2IsMoving", true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        animator.SetBool("backMachine2IsMoving", false);
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

    void Back2TrainingProgress()
    {
        GameManager.instance.back2Training += Time.deltaTime / 100;

        GameManager.instance.back2Training = Mathf.Clamp(GameManager.instance.back2Training, 0, 0.167f);
    }
}
