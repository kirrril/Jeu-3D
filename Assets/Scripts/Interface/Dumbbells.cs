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

        if (!isInteractable && (user.CompareTag("Man") || user.CompareTag("Girl")))
        {
            if (controller.currentCoroutine != null)
            {
                StopCoroutine(controller.currentCoroutine);
                controller.currentCoroutine = null;
                controller.currentCoroutineName = "null";
            }
            controller.isBusy = false;
        }

        if (isInteractable && (user.CompareTag("Man") || user.CompareTag("Girl")))
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


        if (user.CompareTag("Player"))
        {
            return;
        }
    }


    protected IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(true);

        if (user.CompareTag("Man"))
        {
            machineAnimationBool = "dumbbellsAreMoving3";
            machineAnimator.SetBool(machineAnimationBool, true);

            animationBool = "isLiftingDumbbells";
        }
        else if (user.CompareTag("Girl"))
        {
            Debug.Log("Girl");

            SelectTraining();

            Debug.Log("Training: " + training);

            if (training == 1)
            {
                Debug.Log("It is there?");
                machineAnimationBool = "dumbbellsAreMoving1";
                machineAnimator.SetBool(machineAnimationBool, true);

                animationBool = "isLiftingDumbbells";
            }
            else if (training == 2)
            {
                Debug.Log("Here is the problem");
                machineAnimationBool = "dumbbellsAreMoving2";
                machineAnimator.SetBool(machineAnimationBool, true);

                animationBool = "otherWay";
            }
        }
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);

        machineAnimator.SetBool(machineAnimationBool, false);
        yield return new WaitForSeconds(0.2f);
        wall.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        yield return null;
        agent.isStopped = false;
        callBack();
    }


    private void SelectTraining()
    {
        training = Random.Range(1, 3);
    }


    void OnTriggerEnter(Collider other)
    {
        Interact(other.gameObject);
    }



    protected virtual void OnTriggerExit(Collider other)
    {
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        if (other.CompareTag("Man") || other.CompareTag("Girl"))
        {
            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }
        }

        trainingPerson = null;
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
}
