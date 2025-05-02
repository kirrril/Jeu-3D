using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WeightPlate : MonoBehaviour
{
    Coroutine accidentCoroutine;
    string animationBool = "isLying";



    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Head"))
        {
            return;
        }
        else
        {
            Interact(collision.gameObject.transform.parent.gameObject);
        }
    }

    public virtual void Interact(GameObject user)
    {
        if (user.CompareTag("Man") || user.CompareTag("Girl"))
        {
            return;
            // AgentController controller = user.GetComponent<AgentController>();
            // NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
            // GameObject wall = user.transform.Find("Wall").gameObject;
            // NavMeshObstacle obstacle = user.GetComponent<NavMeshObstacle>();

            // if (user.CompareTag("Man"))
            // {
            //     GameObject communicator = user.transform.Find("Communicator").gameObject;
            //     communicator.SetActive(false);
            // }

            // controller.isBusy = true;
            // agent.enabled = false;
            // obstacle.enabled = true;
            // wall.SetActive(true);

            // accidentCoroutine = StartCoroutine(AccidentCorout(user));
        }
        else if (user.CompareTag("Player"))
        {
            GameObject wall = user.transform.Find("Wall").gameObject;
            NavMeshObstacle obstacle = user.GetComponent<NavMeshObstacle>();
            AudioSource ambientSound = GameObject.Find("AmbientSound").GetComponent<AudioSource>();

            ambientSound.Stop();

            user.GetComponentInChildren<Animator>().SetBool(animationBool, true);

            PlayerController.instance.isTraining = true;
            PlayerController.instance.canWalk = false;

            obstacle.enabled = true;
            wall.SetActive(true);

            accidentCoroutine = StartCoroutine(PlayerWasHit(user));
        }
    }

    IEnumerator PlayerWasHit(GameObject user)
    {
        GameObject wall = user.transform.Find("Wall").gameObject;
        NavMeshObstacle obstacle = user.GetComponent<NavMeshObstacle>();
        AudioSource ambientSound = GameObject.Find("AmbientSound").GetComponent<AudioSource>();

        PlayerController.instance.DeadHitByWeightPlate();

        yield return new WaitForSeconds(1f);

        IHM.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        GameManager.instance.currentPlayer.life -= 1;

        PlayerController.instance.StartPosition();

        user.GetComponentInChildren<Animator>().SetBool(animationBool, false);
        PlayerController.instance.isTraining = false;
        PlayerController.instance.canWalk = true;

        obstacle.enabled = false;
        wall.SetActive(false);

        IHM.instance.FadeOut();

        ambientSound.Play();

        StopCoroutine(accidentCoroutine);

        accidentCoroutine = null;
    }



    // IEnumerator AccidentCorout(GameObject user)
    // {
    //     AgentController controller = user.GetComponent<AgentController>();
    //     NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
    //     GameObject wall = user.transform.Find("Wall").gameObject;
    //     NavMeshObstacle obstacle = user.GetComponent<NavMeshObstacle>();
    //     Animator agentAnimator = user.GetComponentInChildren<Animator>();

    //     agentAnimator.SetBool(animationBool, true);
    //     yield return new WaitForSeconds(5f);
    //     agentAnimator.SetBool(animationBool, false);
    //     yield return new WaitForSeconds(0.1f);

    //     agent.enabled = true;
    //     controller.isBusy = false;

    //     if (user.CompareTag("Man"))
    //     {
    //         GameObject communicator = user.transform.Find("Communicator").gameObject;
    //         communicator.SetActive(true);
    //     }

    //     controller.StartMoveToTarget();

    //     wall.SetActive(false);
    //     obstacle.enabled = false;

    //     StopCoroutine(accidentCoroutine);

    //     accidentCoroutine = null;
    // }
}
