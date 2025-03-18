using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class TrainingMachineBase : MonoBehaviour, IInteractable
{
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
    }


    protected virtual void Update()
    {
        WaterManagement();
    }


    public virtual void Interact(GameObject user)
    {
        AgentController controller = user.GetComponent<AgentController>();

        if (!isInteractable && user.CompareTag("Man") || !isInteractable && user.CompareTag("Girl"))
        {
            if (controller.currentCoroutine != null)
            {
                StopCoroutine(controller.currentCoroutine);
                controller.currentCoroutine = null;
                controller.currentCoroutineName = "null";
            }
            controller.isBusy = false;
        }

        if (isInteractable && user.CompareTag("Man") || isInteractable && user.CompareTag("Girl"))
        {
            if (controller.currentCoroutine != null)
            {
                StopCoroutine(controller.currentCoroutine);
                controller.currentCoroutine = null;
                controller.currentCoroutineName = "null";
            }
            controller.isBusy = true;

            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
            agent.enabled = false;

            trainingPerson = user;

            TakePlace();

            trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
        }


        if (!isInteractable && user.CompareTag("Player"))
        {
            return;
        }

        if (isInteractable && user.CompareTag("Player"))
        {
            trainingPerson = user;

            TakePlace();

            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

            StopTrainingButtonOn();
        }
    }


    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(true);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        yield return new WaitForSeconds(0.2f);
        // user.GetComponentInChildren<Animator>().SetFloat("MovementSpeed", 0.2f);
        // yield return new WaitForSeconds(0.2f);
        wall.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        yield return null;
        agent.isStopped = false;
        callBack();
    }




    protected virtual void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        if (trainingPerson.CompareTag("Player"))
        {
            PlayerController.instance.isTraining = false;
            other.gameObject.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        }

        if (other.gameObject.CompareTag("Man") || other.gameObject.CompareTag("Girl"))
        {
            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }
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

    protected void TakePlace()
    {
        trainingPerson.transform.position = trainingPosition.position;
        trainingPerson.transform.rotation = trainingPosition.rotation;
    }

    protected void LeavePlace()
    {
        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;
    }


    protected void PlayerLeavePlace()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = stopTrainingPosition.position;
        player.transform.rotation = stopTrainingPosition.rotation;
    }


    void OnButtonClick()
    {
        PlayerLeavePlace();
        StopTrainingButtonOff();
    }


    protected void WaterManagement()
    {
        if (PlayerController.instance.isTraining == true)
        {
            float waterLoss = Time.deltaTime / 100;

            GameManager.instance.currentPlayer.water -= waterLoss;
        }

        if (GameManager.instance.currentPlayer.water <= 0)
        {
            PlayerController.instance.StartPosition();

            IHM.instance.stopTrainingButton.gameObject.SetActive(false);

            GameManager.instance.currentPlayer.life--;

            GameManager.instance.currentPlayer.water = 0.5f;
        }
    }
}
