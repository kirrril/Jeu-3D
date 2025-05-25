using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class PullUpBar : MonoBehaviour
{
    bool thisPullUpBar;

    [SerializeField]
    Transform trainingPosition1;

    [SerializeField]
    Transform trainingPosition2;

    [SerializeField]
    Transform trainingPosition3;

    [SerializeField]
    Transform trainingPosition4;

    [SerializeField]
    Transform stopTrainingPosition1;

    [SerializeField]
    Transform stopTrainingPosition2;

    [SerializeField]
    Transform stopTrainingPosition3;

    [SerializeField]
    Transform stopTrainingPosition4;

    [SerializeField]
    NavMeshObstacle obstacle;

    [SerializeField]
    GameObject wall;

    [SerializeField]
    Animator machineAnimator;

    [SerializeField]
    protected AudioSource trainingAudio;

    [SerializeField]
    protected AudioSource ambientSound;


    GameObject trainingPerson;

    Coroutine trainingCoroutine;

    protected Coroutine thirstyCoroutine = null;


    public bool isInteractable { get { return trainingPerson == null; } set { } }

    public bool isInteracting { get { return trainingPerson != null; } }


    float trainingDuration;

    string animationBool;


    void Start()
    {
        trainingDuration = 10.0f;

        wall.SetActive(false);

        obstacle.enabled = false;
    }

    void Update()
    {
        if (thisPullUpBar)
        {
            PullUpsTrainingProgress();
            WaterManagement();
            DisplayMachineWarning();
        }
    }

    public void DisplayMachineWarning()
    {
        if (GameManager.instance.pullUpsTraining >= 0.167f)
        {
            if (IHM.instance.contextMessageCoroutName != "PullUpsTrainingCompleted")
            {
                IHM.instance.contextMessageCorout = StartCoroutine(PullUpsTrainingCompletedWarning());
                IHM.instance.contextMessageCoroutName = "PullUpsTrainingCompleted";
            }
            trainingAudio.Stop();
        }
    }

    public IEnumerator PullUpsTrainingCompletedWarning()
    {
        IHM.instance.contextMessage.text = "PULL-UPS TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "PULL-UPS TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "PULL-UPS TRAINING COMPLETED";

        yield return new WaitForSeconds(1f);

        IHM.instance.contextMessage.text = "";
    }

    void WaterManagement()
    {
        if (GameManager.instance.pullUpsTraining < 0.167f)
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

    protected IEnumerator ThirstyCorout()
    {
        StartCoroutine(IHM.instance.ThirstyDeathCorout());

        yield return new WaitForSeconds(2f);

        PlayerController.instance.isTraining = false;

        PlayerController.instance.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        StopTrainingButtonOff();

        PlayerController.instance.StartPosition();

        obstacle.enabled = false;
        wall.SetActive(false); //////////////////?????????????????????

        // trainingPerson = null; ////////////////////// ??????????????????????????????????

        yield return new WaitForSeconds(2f);

        ambientSound.Play();

        GameManager.instance.currentPlayer.life -= 1;

        GameManager.instance.currentPlayer.water = 0.5f;
    }

    void PullUpsTrainingProgress()
    {
        GameManager.instance.pullUpsTraining += Time.deltaTime / 300;

        GameManager.instance.pullUpsTraining = Mathf.Clamp(GameManager.instance.pullUpsTraining, 0, 0.167f);
    }

    void Interact(GameObject user)
    {
        AgentController controller = user.GetComponent<AgentController>();
        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

        if (!isInteractable)
        {
            return;
        }

        if (user.CompareTag("Player"))
        {
            if (!isInteractable)
            {
                return;
            }

            thisPullUpBar = true;

            PlayerController.instance.isTraining = true;

            if (GameManager.instance.pullUpsTraining <= 0.167f)
            {
                IHM.instance.DisplayWaterWarning();
            }

            if (GameManager.instance.pullUpsTraining < 0.056f)
            {
                animationBool = "isMakingAustralianPullUps";

                TakePlace(user, trainingPosition1);

                obstacle.enabled = true;
                wall.SetActive(true);

                trainingAudio.Play();

                ambientSound.Stop();

                user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

                // PlayerController.instance.isTraining = true;

                StopTrainingButtonOn(user, stopTrainingPosition1);

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 0.4f, 0f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 1.2f, 2.3f);
            }
            else if (GameManager.instance.pullUpsTraining > 0.056f && GameManager.instance.pullUpsTraining < 0.11)
            {
                animationBool = "isMakingPullUps";

                TakePlace(user, trainingPosition2);

                obstacle.enabled = true;
                wall.SetActive(true);

                trainingAudio.Play();

                ambientSound.Stop();


                machineAnimator.SetBool("RubberIsPulled", true);

                user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

                

                // PlayerController.instance.isTraining = true;

                StopTrainingButtonOn(user, stopTrainingPosition2);

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 1.2f, -0.5f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 2.2f, 2.2f);
            }
            else
            {
                animationBool = "isMakingPullUps2";

                TakePlace(user, trainingPosition4);

                obstacle.enabled = true;
                wall.SetActive(true);

                trainingAudio.Play();

                ambientSound.Stop();

                user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

                // PlayerController.instance.isTraining = true;

                StopTrainingButtonOn(user, stopTrainingPosition4);

                Transform cameraTarget = GameObject.Find("CameraTarget").transform;
                cameraTarget.localPosition = new Vector3(0f, 1.6f, -0.5f);

                CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
                CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
                playerTransposer.m_FollowOffset = new Vector3(0f, 1f, 2.2f);
            }
        }
        else if (user.CompareTag("Man"))
        {
            trainingPerson = user;

            controller.isBusy = true;

            agent.enabled = false;

            TakePlace(user, trainingPosition3);

            if (user.CompareTag("Man"))
            {
                GameObject communicator = user.transform.Find("Communicator").gameObject;
                communicator.SetActive(false);
            }

            obstacle.enabled = true;
            wall.SetActive(true);

            trainingCoroutine = StartCoroutine(TrainingCorout(user, stopTrainingPosition3));
        }
        else if (user.CompareTag("Girl"))
        {
            trainingPerson = user;

            controller.isBusy = true;

            agent.enabled = false;

            TakePlace(user, trainingPosition1);

            obstacle.enabled = true;
            wall.SetActive(true);

            trainingCoroutine = StartCoroutine(TrainingCorout(user, stopTrainingPosition1));
        }
    }

    protected IEnumerator TrainingCorout(GameObject user, Transform stopTrainingPosition)
    {
        animationBool = "isMakingPullUps";

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        // machineAnimator.SetBool(machineAnimationBool, false);

        trainingCoroutine = null;

        LeavePlace(user, stopTrainingPosition);

        if (user.CompareTag("Man"))
        {
            GameObject communicator = user.transform.Find("Communicator").gameObject;
            communicator.SetActive(true);
        }

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        yield return new WaitForSeconds(2f);
        wall.SetActive(false);
        obstacle.enabled = false;

        trainingPerson = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man") || other.CompareTag("Girl") || other.CompareTag("Player"))
        {
            Interact(other.gameObject);
        }
        else
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != trainingPerson)
        {
            return;
        }
    }

    void TakePlace(GameObject trainingPerson, Transform trainingPosition)
    {
        trainingPerson.transform.position = trainingPosition.position;
        trainingPerson.transform.rotation = trainingPosition.rotation;
    }

    void LeavePlace(GameObject trainingPerson, Transform stopTrainingPosition)
    {
        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();

        if (!agent.enabled)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        controller.StartMoveToTarget();

        // trainingPerson = null;
    }

    protected void StopTrainingButtonOn(GameObject user, Transform stopTrainingPosition)
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        IHM.instance.stopTrainingButton.onClick.AddListener(() => OnButtonClick(user, stopTrainingPosition));
    }

    protected void PlayerLeavePlace(GameObject user, Transform stopTrainingPosition)
    {
        thisPullUpBar = false;

        obstacle.enabled = false;
        wall.SetActive(false);

        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool("RubberIsPulled", false);

        PlayerController.instance.isTraining = false;

        user.transform.position = stopTrainingPosition.position;
        user.transform.rotation = stopTrainingPosition.rotation;

        trainingAudio.Stop();
        ambientSound.Play();

        Transform cameraTarget = GameObject.Find("CameraTarget").transform;
        cameraTarget.localPosition = new Vector3(0f, 1.385f, 0.639f);

        CinemachineVirtualCamera playerCam = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
        CinemachineTransposer playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        playerTransposer.m_FollowOffset = new Vector3(0f, 2f, -1f);
    }

    void OnButtonClick(GameObject user, Transform stopTrainingPosition)
    {
        PlayerLeavePlace(user, stopTrainingPosition);
        StopTrainingButtonOff();
    }

    void StopTrainingButtonOff()
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(false);
    }
}
