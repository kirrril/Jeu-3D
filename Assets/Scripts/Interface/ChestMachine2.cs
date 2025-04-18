using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class ChestMachine2 : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isTrainingChest_2";
    }

    protected override void Update()
    {
        base.Update();

        Chest2TrainingProgress();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Player"))
        {
            Transform cameraTarget = GameObject.Find("CameraTarget").transform;
            cameraTarget.localPosition = new Vector3(0f, 0.6f, -0.5f);

            CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
            playerTransposer.m_FollowOffset = new Vector3(0f, 1.8f, 1.8f);

            CinemachineVirtualCamera observerCam = GameObject.Find("ObserverCam").GetComponent<CinemachineVirtualCamera>();
            CinemachineTransposer observerTransposer = observerCam.GetCinemachineComponent<CinemachineTransposer>();
            observerTransposer.m_FollowOffset = new Vector3(2f, 3.3f, 5f);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.CompareTag("Player"))
        {
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

        AgentController controller = user.GetComponent<AgentController>();
        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

        if (controller.currentCoroutine != null)
        {
            StopCoroutine(controller.currentCoroutine);
            controller.currentCoroutine = null;
            controller.currentCoroutineName = "null";
        }

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

        trainingCoroutine = StartCoroutine(TrainingCorout(user/*, LeavePlace*/));
    }

    protected override IEnumerator TrainingCorout(GameObject user/*, System.Action callBack*/)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool("chestMachine2IsMoving", true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool("chestMachine2IsMoving", false);
        yield return new WaitForSeconds(0.1f);

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        // trainingCoroutine = null;
        // callBack();

        // NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        // agent.enabled = true;
        // agent.isStopped = false;

        // yield return new WaitForSeconds(2f);
        // GameObject wall = transform.Find("Wall").gameObject;
        // wall.SetActive(false);
        // NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        // obstacle.enabled = false;
    }

    void Chest2TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.chest2Training += Time.deltaTime / 500;

            GameManager.instance.chest2Training = Mathf.Clamp(GameManager.instance.chest2Training, 0, 0.35f);
        }
    }
}
