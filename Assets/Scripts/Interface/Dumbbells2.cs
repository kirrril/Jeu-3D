using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dumbbells2 : MonoBehaviour
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

        if (user.CompareTag("Man"))
        {
            GameObject communicator = user.transform.Find("Communicator").gameObject;
            communicator.SetActive(false);
        }

        obstacle.enabled = true;
        wall.SetActive(true);

        trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
    }

    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        machineAnimationBool = "Dumbbells2AreMoving";
        machineAnimator.SetBool(machineAnimationBool, true);

        animationBool = "isLiftnigDumbbells4";

        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool(machineAnimationBool, false);

        trainingCoroutine = null;
        callBack();

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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            Interact(other.gameObject);
        }
        else
        {
            return;
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
