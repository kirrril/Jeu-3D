using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class TrainingMachineBase : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected AudioSource trainingAudio;

    [SerializeField]
    AudioSource ambientSound;

    [SerializeField]
    protected Transform trainingPosition;
    [SerializeField]
    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;

    protected float trainingDuration = 5.0f;

    protected float thisMachineTraining = 0;

    protected string animationBool;

    protected Coroutine trainingCoroutine;

    public virtual bool isInteractable { get { return trainingPerson == null; } set { } }

    public bool isInteracting { get { return trainingPerson != null; } }


    protected virtual void Start()
    {
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        GameObject ambientSoundSource = GameObject.Find("AmbientSound");
        ambientSound = ambientSoundSource.GetComponent<AudioSource>();
    }


    protected virtual void Update()
    {
        WaterManagement();
    }


    public virtual void Interact(GameObject user)
    {
        if (user.CompareTag("Man") || user.CompareTag("Girl"))
        {
            AgentController controller = user.GetComponent<AgentController>();
            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

            if (!isInteractable)
            {
                if (controller.currentCoroutine != null)
                {
                    StopCoroutine(controller.currentCoroutine);
                    controller.currentCoroutine = null;
                    controller.currentCoroutineName = "null";
                }

                controller.StartMoveToTarget();
            }

            trainingPerson = user;

            controller.isBusy = true;

            // if (controller.currentCoroutine != null)
            // {
            //     StopCoroutine(controller.currentCoroutine);
            //     controller.currentCoroutine = null;
            //     controller.currentCoroutineName = "null";
            // }
            agent.enabled = false;

            TakePlace(user);

            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
        }
        else if (user.CompareTag("Player"))
        {
            if (!isInteractable)
            {
                return;
            }

            trainingPerson = user;

            TakePlace(user);

            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            if (trainingAudio != null) trainingAudio.Play();
            if (ambientSound != null) ambientSound.Stop();

            IHM.instance.DisplayWaterWarning();
            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
            PlayerController.instance.isTraining = true;

            StopTrainingButtonOn();
        }
    }


    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        yield return new WaitForSeconds(0.1f);

        // yield return new WaitForSeconds(0.2f);
        // 
        // obstacle.enabled = false;
        // 
        // wall.SetActive(false);
        // NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        // agent.enabled = true;
        // yield return null;
        // agent.isStopped = false;
        // callBack();

        trainingCoroutine = null;
        callBack();

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        yield return new WaitForSeconds(2f);
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl") || other.CompareTag("Man") || other.CompareTag("Player"))
        {
            Interact(other.gameObject);
        }
    }




    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject != trainingPerson)
        {
            return;
        }

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        if (other.CompareTag("Man") || other.CompareTag("Girl"))
        {
            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;

                Animator userAnimator = trainingPerson.GetComponentInChildren<Animator>();
                userAnimator.SetBool(animationBool, false);

                NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
                agent.enabled = true;
                agent.isStopped = false;
            }
        }
        else if (other.CompareTag("Player"))
        {
            PlayerController.instance.isTraining = false;
            trainingPerson.GetComponentInChildren<Animator>().SetBool(animationBool, false);
            StopTrainingButtonOff();
        }

        trainingPerson = null;
    }


    void StopTrainingButtonOn()
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(true);

        IHM.instance.stopTrainingButton.onClick.AddListener(OnButtonClick);
    }

    void StopTrainingButtonOff()
    {
        IHM.instance.stopTrainingButton.gameObject.SetActive(false);
    }

    protected void TakePlace(GameObject trainingPerson)
    {
        trainingPerson.transform.position = trainingPosition.position;
        trainingPerson.transform.rotation = trainingPosition.rotation;
    }

    protected void LeavePlace()
    {
        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        if (!agent.enabled)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }
        controller.StartMoveToTarget();

        trainingPerson = null;
    }


    protected void PlayerLeavePlace()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(true);

        GameObject player = GameObject.Find("Player");
        player.transform.position = stopTrainingPosition.position;
        player.transform.rotation = stopTrainingPosition.rotation;
        trainingAudio.Stop();
        ambientSound.Play();
    }


    void OnButtonClick()
    {
        PlayerLeavePlace();
        StopTrainingButtonOff();
    }


    void WaterManagement()
    {
        if (PlayerController.instance.isTraining == true && trainingPerson == PlayerController.instance.gameObject)
        {
            float waterLoss = Time.deltaTime / 20;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0 && trainingPerson == PlayerController.instance.gameObject)
        {
            if (trainingAudio != null)
            {
                trainingAudio.Stop();
            }

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            StartCoroutine(ThirstyCorout());

            if (ambientSound != null) ambientSound.Play();

            GameManager.instance.currentPlayer.life -= 1;

            GameManager.instance.currentPlayer.water = 0.5f;
        }
    }

    IEnumerator ThirstyCorout()
    {
        StartCoroutine(IHM.instance.ThirstyDeathCorout());

        yield return new WaitForSeconds(3f);

        PlayerController.instance.isTraining = false;

        PlayerController.instance.StartPosition();

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }
}
