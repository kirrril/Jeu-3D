using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dumbbells : MonoBehaviour
{
    [SerializeField]
    Transform trainingPosition;

    [SerializeField]
    Transform stopTrainingPosition;

    GameObject trainingPerson;

    Coroutine trainingCoroutine;

    public bool isInteractable { get { return trainingPerson == null; } set { } }

    public bool isInteracting { get { return trainingPerson != null; } }

    Animator machineAnimator;

    int training;
    float trainingDuration;

    string animationBool;
    string machineAnimationBool;

    void Start()
    {
        trainingDuration = 10.0f;

        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        machineAnimator = GetComponentInChildren<Animator>();
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

        if (!user.CompareTag("Man") && !user.CompareTag("Girl"))
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

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(true);

        trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
    }

    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        GameObject wall = transform.Find("Wall").gameObject;

        if (user.CompareTag("Man"))
        {
            machineAnimationBool = "dumbbellsAreMoving3";
            machineAnimator.SetBool(machineAnimationBool, true);

            animationBool = "isLiftingDumbbells";
        }
        else if (user.CompareTag("Girl"))
        {
            SelectTraining();

            if (training == 1)
            {
                machineAnimationBool = "dumbbellsAreMoving1";
                machineAnimator.SetBool(machineAnimationBool, true);

                animationBool = "isLiftingDumbbells";
            }
            else if (training == 2)
            {
                machineAnimationBool = "dumbbellsAreMoving2";
                machineAnimator.SetBool(machineAnimationBool, true);

                animationBool = "otherWay";
            }
        }
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


    private void SelectTraining()
    {
        training = Random.Range(1, 3);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man") || other.CompareTag("Girl"))
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
