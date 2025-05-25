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
    protected AudioSource ambientSound;

    [SerializeField]
    protected Transform trainingPosition;
    [SerializeField]
    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;

    protected float trainingDuration = 5.0f;

    protected float thisMachineTraining = 0;

    protected string animationBool;

    protected string machineAnimationBool;

    protected Coroutine trainingCoroutine;

    protected Coroutine thirstyCoroutine = null;

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

    }


    public virtual void Interact(GameObject user)
    {
        if (user.CompareTag("Man") || user.CompareTag("Girl"))
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

            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
            PlayerController.instance.isTraining = true;

            Animator animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool(machineAnimationBool, true);
            }


            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            trainingAudio.Play();

            ambientSound.Stop();

            StopTrainingButtonOn();
        }
    }

    protected virtual IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool(machineAnimationBool, true);
        }

        yield return new WaitForSeconds(trainingDuration);

        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        if (animator != null)
        {
            animator.SetBool(machineAnimationBool, false);
        }

        yield return new WaitForSeconds(0.1f);

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        user.transform.position = stopTrainingPosition.position;
        user.transform.rotation = stopTrainingPosition.rotation;

        if (user.CompareTag("Man"))
        {
            GameObject communicator = user.transform.Find("Communicator").gameObject;
            communicator.SetActive(true);
        }

        AgentController controller = user.GetComponent<AgentController>();
        controller.isBusy = false;

        controller.StartMoveToTarget();

        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl") || other.CompareTag("Man"))
        {
            Interact(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(GiveWay(other.gameObject));
        }
    }

    IEnumerator GiveWay(GameObject player)
    {
        yield return new WaitForSeconds(0.1f);

        Interact(player);
    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }


    protected void StopTrainingButtonOn()
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

    protected virtual void PlayerLeavePlace()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        PlayerController.instance.isTraining = false;

        GameObject player = GameObject.Find("Player");

        player.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool(machineAnimationBool, false);
        }

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

    protected IEnumerator ThirstyCorout()
    {
        StartCoroutine(IHM.instance.ThirstyDeathCorout());

        yield return new WaitForSeconds(2f);

        PlayerController.instance.isTraining = false;

        PlayerController.instance.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        StopTrainingButtonOff();

        PlayerController.instance.StartPosition();

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        ambientSound.Play();

        GameManager.instance.currentPlayer.life -= 1;

        GameManager.instance.currentPlayer.water = 0.5f;

        thirstyCoroutine = null;
    }
}