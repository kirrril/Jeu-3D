using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barbell : TrainingMachineBase, IInteractable
{
    protected override void Start()
    {
        base.Start();

        trainingDuration = 10.0f;

        animationBool = "isPushingBarbell";
    }

    protected override void Update()
    {
        base.Update();

        TrainingProgress();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Man"))
        {
            Interact(other.gameObject);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellIsMoving", true);
        }
        else if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);

            Animator animator = GetComponentInChildren<Animator>();
            animator.SetBool("barbellIsMoving", true);
        }
        else
        {
            GameObject wall = transform.Find("Wall").gameObject;
            wall.SetActive(true);
        }
    }


    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("barbellIsMoving", false);
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


    void TrainingProgress()
    {
        if (PlayerController.instance.isTraining)
        {
            GameManager.instance.bikeTraining += Time.deltaTime / 500;

            GameManager.instance.bikeTraining = Mathf.Clamp(GameManager.instance.treadmillTraining, 0, 0.35f);
        }
    }
}