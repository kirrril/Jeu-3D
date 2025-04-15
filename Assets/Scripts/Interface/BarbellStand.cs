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
        if (!other.CompareTag("Man"))
        {
            return;
        }
        else
        {
            Interact(other.gameObject);

            Animator machineAnimator = GetComponentInChildren<Animator>();
            machineAnimator.SetBool("barbellStandIsMoving", true);

            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Man"))
        {
            NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
            obstacle.enabled = false;

            if (trainingCoroutine != null)
            {
                StopCoroutine(trainingCoroutine);
                trainingCoroutine = null;
            }

            Animator machineAnimator = GetComponentInChildren<Animator>();
            machineAnimator.SetBool("barbellStandIsMoving", false);

            trainingPerson = null;

            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(false);

            AgentController agentController = other.GetComponent<AgentController>();

            if (agentController.currentCoroutineName != "MoveToTarget")
            {
                if (agentController.currentCoroutine != null)
                {
                    StopCoroutine(agentController.currentCoroutine);
                }

                agentController.StartMoveToTarget();        
            }
            
        }
    }


    public override void Interact(GameObject user)
    {
        if (!user.CompareTag("Man"))
        {
            return;
        }

        base.Interact(user);
    }
}
