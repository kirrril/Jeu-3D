using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarbellStand : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isBarbellSquatting";

        machineAnimationBool = "barbellStandIsMoving";
    }

    public override void Interact(GameObject user)
    {
        if (!user.CompareTag("Man"))
        {
            return;
        }

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

    protected override IEnumerator TrainingCorout(GameObject user)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool(machineAnimationBool, true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool(machineAnimationBool, false);
        yield return new WaitForSeconds(0.1f);

        AgentController controller = trainingPerson.GetComponent<AgentController>();
        controller.isBusy = false;

        NavMeshAgent agent = trainingPerson.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        trainingPerson.transform.position = stopTrainingPosition.position;
        trainingPerson.transform.rotation = stopTrainingPosition.rotation;

        if (user.CompareTag("Man"))
        {
            GameObject communicator = user.transform.Find("Communicator").gameObject;
            communicator.SetActive(true);
        }

        controller.StartMoveToTarget();

        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

        trainingPerson = null;
    }
}
