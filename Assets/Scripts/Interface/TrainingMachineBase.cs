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

    }


    public virtual void Interact(GameObject user)
    {
        if (user.CompareTag("Man") || user.CompareTag("Girl"))
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
            PlayerController.instance.isTraining = true;

            StopTrainingButtonOn();
        }
    }

    protected virtual IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        yield return new WaitForSeconds(0.1f);



        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        user.transform.position = stopTrainingPosition.position;
        user.transform.rotation = stopTrainingPosition.rotation;

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
        if (other.CompareTag("Girl") || other.CompareTag("Man") || other.CompareTag("Player"))
        {
            Interact(other.gameObject);
        }
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

    protected void PlayerLeavePlace()
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        PlayerController.instance.isTraining = false;

        GameObject player = GameObject.Find("Player");

        player.GetComponentInChildren<Animator>().SetBool(animationBool, false);

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

        yield return new WaitForSeconds(3f);

        PlayerController.instance.isTraining = false;

        PlayerController.instance.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        StopTrainingButtonOff();

        PlayerController.instance.StartPosition();

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }
}