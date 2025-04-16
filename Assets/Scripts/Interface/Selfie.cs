using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Selfie : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 3.0f;

        animationBool = "isShowingOff";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Girl") || other.CompareTag("Man"))
        {
            Interact(other.gameObject);
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

            // if (controller.currentCoroutine != null)
            // {
            //     StopCoroutine(controller.currentCoroutine);
            //     controller.currentCoroutine = null;
            //     controller.currentCoroutineName = "null";
            // }
            agent.enabled = false;

            TakePlace(user);

            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = true;
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);

            trainingCoroutine = StartCoroutine(TrainingCorout(user, LeavePlace));
        }
    }
}
