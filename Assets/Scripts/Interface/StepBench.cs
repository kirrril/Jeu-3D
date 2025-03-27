using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StepBench : MonoBehaviour
{
    [SerializeField]
    protected Transform trainingPosition;
    [SerializeField]
    protected Transform stopTrainingPosition;

    protected GameObject trainingPerson;
    protected float trainingDuration = 5.0f;
    protected string animationBool;
    protected Coroutine trainingCoroutine;

    public virtual bool isInteractable { get { return trainingPerson == null; } set { } }
    public bool isInteracting { get { return trainingPerson != null; } }

    protected virtual void Start()
    {
        GameObject wall = transform.Find("Wall")?.gameObject;
        if (wall == null)
        {
            Debug.LogWarning("Wall not found in YogaPad!", this);
        }
        else
        {
            wall.SetActive(false);
        }

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle == null)
        {
            Debug.LogWarning("NavMeshObstacle not found on YogaPad!", this);
        }
        else
        {
            obstacle.enabled = false;
        }

        trainingDuration = 10.0f;
        animationBool = "isStepping";
    }

    void Interact(GameObject user)
    {
        if (!user.CompareTag("Girl")) return; // Ignorer si ce n’est pas une "Girl"

        AgentControllerYouWin controller = user.GetComponent<AgentControllerYouWin>();
        if (controller == null) return;

        // Si la station est occupée, libérer l’agent qui essaie d’interagir
        if (!isInteractable)
        {
            if (controller.currentCoroutine != null)
            {
                controller.StopCoroutine(controller.currentCoroutine);
                controller.currentCoroutine = null;
                controller.currentCoroutineName = "null";
            }
            controller.isBusy = false;
            return;
        }

        // Si la station est libre, commencer l’entraînement
        if (controller.currentCoroutine != null)
        {
            controller.StopCoroutine(controller.currentCoroutine);
            controller.currentCoroutine = null;
            controller.currentCoroutineName = "null";
        }
        controller.isBusy = true;

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        trainingPerson = user;
        TakePlace();

        trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
    }

    protected IEnumerator TrainingCorout(GameObject user, Action callBack)
    {
        if (user == null) yield break;

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle != null) obstacle.enabled = true;

        GameObject wall = transform.Find("Wall")?.gameObject;
        if (wall != null) wall.SetActive(true);

        Animator animator = user.GetComponentInChildren<Animator>();
        if (animator != null) animator.SetBool(animationBool, true);

        yield return new WaitForSeconds(trainingDuration);

        if (animator != null) animator.SetBool(animationBool, false);
        yield return new WaitForSeconds(0.2f);

        if (wall != null) wall.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        if (trainingPerson != null) // Vérifier que trainingPerson est toujours valide
        {
            NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
                agent.isStopped = false;
            }
        }

        yield return null;
        callBack?.Invoke();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Girl") || other.gameObject != trainingPerson) return;

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle != null) obstacle.enabled = false;

        if (trainingCoroutine != null)
        {
            StopCoroutine(trainingCoroutine);
            trainingCoroutine = null;
        }

        trainingPerson = null;
    }

    protected void TakePlace()
    {
        if (trainingPerson != null)
        {
            trainingPerson.transform.position = trainingPosition.position;
            trainingPerson.transform.rotation = trainingPosition.rotation;
        }
    }

    protected void LeavePlace()
    {
        if (trainingPerson != null)
        {
            trainingPerson.transform.position = stopTrainingPosition.position;
            trainingPerson.transform.rotation = stopTrainingPosition.rotation;

            AgentControllerYouWin controller = trainingPerson.GetComponent<AgentControllerYouWin>();
            if (controller != null)
            {
                controller.isBusy = false;
            }
        }
    }
}