using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squatting : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isSquatting";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man") || other.CompareTag("Girl"))
        {
            Interact(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            return;
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }

        if (other.CompareTag("Man") || other.CompareTag("Girl"))
        {
            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = false;

            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }

            trainingPerson = null;
        }
    }

    public override void Interact(GameObject user)
    {
        if (user.CompareTag("Player"))
        {
            return;
        }

        if (user.CompareTag("Man") || user.CompareTag("Girl"))
        {
            AgentController controller = user.GetComponent<AgentController>();

            if (user.CompareTag("Man") || user.CompareTag("Girl"))
            {
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

                    NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
                    agent.isStopped = true;
                    agent.enabled = false;

                    trainingPerson = user;

                    TakePlace();

                    trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
                }
            }

        }
    }
}
