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



    protected virtual void Update()
    {
        WaterManagement();
    }


    public virtual void Interact(GameObject user)
    {
        if (!isInteractable) return;

        if (user.CompareTag("Agent"))
        {
            AgentController controller = user.GetComponent<AgentController>();

            if (controller.currentCoroutine != null)
            {
                StopCoroutine(controller.currentCoroutine);
            }

            controller.isBusy = true;

            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
            agent.isStopped = true;
            agent.enabled = false;
        }

        trainingPerson = user;

        TakePlace();

        if (trainingPerson.CompareTag("Player"))
        {
            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

            StopTrainingButtonOn();
        }

        if (trainingPerson.CompareTag("Agent"))
        {
            trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
        }

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
    }


    IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        yield return new WaitForSeconds(0.2f);
        user.GetComponentInChildren<Animator>().SetFloat("MovementSpeed", 0.2f);
        yield return new WaitForSeconds(0.2f);

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        callBack();
        agent.Warp(stopTrainingPosition.position);
        yield return null;
        agent.isStopped = false;
        Debug.Log($"{trainingPerson.name} leaving machine, position: {trainingPerson.transform.position}, agent enabled: {agent.enabled}");

        yield return new WaitForSeconds(1f);
        AgentController controller = user.GetComponent<AgentController>();
        controller.isBusy = false;
        controller.currentCoroutine = StartCoroutine(controller.MoveToTarget());
        controller.currentCoroutineName = "MoveToTarget";
    }


    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }


    void OnTriggerExit(Collider other)
    {
        if (trainingPerson.CompareTag("Player"))
        {
            PlayerController.instance.isTraining = false;
            other.gameObject.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        }

        if (other.gameObject.CompareTag("Agent"))
        {
            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }

            AgentController controller = other.gameObject.GetComponent<AgentController>();

            if (controller is ManController manController)
            {
                manController.playerWasAttacked = false;
            }
        }

        trainingPerson = null;

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
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

    void TakePlace()
    {
        trainingPerson.transform.position = trainingPosition.position;
        trainingPerson.transform.rotation = trainingPosition.rotation;
    }

    protected void LeavePlace()
    {
        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;
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
