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

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            Interact(other.gameObject);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellStandIsMoving", true);
        }
        else
        {
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Man"))
        {
            NavMeshObstacle obstacle = GetComponentInParent<NavMeshObstacle>();
            obstacle.enabled = false;

            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellStandIsMoving", false);

            trainingPerson = null;
        }
        else
        {
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(false);
        }
    }


    public override void Interact(GameObject user)
    {
        if (user.CompareTag("Man"))
        {
            AgentController controller = user.transform.parent.GetComponent<AgentController>();

            if (!isInteractable)
            {
                if (controller.currentCoroutine != null)
                {
                    StopCoroutine(controller.currentCoroutine);
                    controller.currentCoroutine = null;
                    controller.currentCoroutineName = "null";
                }
                controller.isBusy = false;
            }

            if (isInteractable)
            {
                if (controller.currentCoroutine != null)
                {
                    StopCoroutine(controller.currentCoroutine);
                    controller.currentCoroutine = null;
                    controller.currentCoroutineName = "null";
                }
                controller.isBusy = true;

                NavMeshAgent agent = user.transform.parent.GetComponent<NavMeshAgent>();
                agent.isStopped = true;
                agent.enabled = false;

                trainingPerson = user.transform.parent.gameObject;

                TakePlace();

                trainingCoroutine = StartCoroutine(TrainingCorout(user.transform.parent.gameObject, LeavePlace));
            }
        }
        else
        {
            return;
        }
    }

}
