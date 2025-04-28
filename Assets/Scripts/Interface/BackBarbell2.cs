using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackBarbell2 : MonoBehaviour
{
    [SerializeField]
    Transform trainingPosition;

    [SerializeField]
    Transform stopTrainingPosition;

    [SerializeField]
    GameObject wall;

    [SerializeField]
    Animator machineAnimator;

    [SerializeField]
    NavMeshObstacle obstacle;

    GameObject trainingPerson;

    Coroutine trainingCoroutine;

    public bool isInteractable { get { return trainingPerson == null; } set { } }

    public bool isInteracting { get { return trainingPerson != null; } }


    int training;
    float trainingDuration;

    string animationBool;
    string machineAnimationBool;

    void Start()
    {
        trainingDuration = 10.0f;

        wall.SetActive(false);

        obstacle.enabled = false;
    }

    void Interact(GameObject user)
    {
        AgentController controller = user.GetComponent<AgentController>();
        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();

        // if (controller.currentCoroutine != null)
        // {
        //     StopCoroutine(controller.currentCoroutine);
        //     controller.currentCoroutine = null;
        //     controller.currentCoroutineName = "null";
        // }

        if (!user.CompareTag("Man"))
        {
            return;
        }

        if (!isInteractable)
        {
            return;
        }

        trainingPerson = user;

        controller.isBusy = true;

        agent.enabled = false;

        TakePlace(user);

        obstacle.enabled = true;
        wall.SetActive(true);

        trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
    }

    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        machineAnimationBool = "BackBarbell2IsMoving";
        machineAnimator.SetBool(machineAnimationBool, true);

        animationBool = "isPullingBackBarbell2";

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool(machineAnimationBool, false);

        trainingCoroutine = null;
        callBack();

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        yield return new WaitForSeconds(2f);
        wall.SetActive(false);
        obstacle.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man"))
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

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();

        if (!agent.enabled)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }
        controller.StartMoveToTarget();

        trainingPerson = null;
    }
}
