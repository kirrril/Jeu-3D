using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarbellStand : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 8.0f;

        animationBool = "isBarbellSquatting";
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
            if (controller.currentCoroutine != null)
            {
                StopCoroutine(controller.currentCoroutine);
                controller.currentCoroutine = null;
                controller.currentCoroutineName = "null";
            }

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

        trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
    }

    protected override IEnumerator TrainingCorout(GameObject user, System.Action callBack)
    {
        user.GetComponentInChildren<Animator>().SetBool(animationBool, true);
        Animator machineAnimator = GetComponentInChildren<Animator>();
        machineAnimator.SetBool("barbellStandIsMoving", true);
        yield return new WaitForSeconds(trainingDuration);
        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        machineAnimator.SetBool("barbellStandIsMoving", false);
        yield return new WaitForSeconds(0.1f);

        trainingCoroutine = null;
        callBack();

        NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.isStopped = false;

        yield return new WaitForSeconds(2f);
        GameObject wall = transform.Find("Wall").gameObject;
        wall.SetActive(false);
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }
}
