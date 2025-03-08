using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ManController : AgentController
{
    public bool playerWasAttacked;

    protected override IEnumerator ChaseFleePlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > 1f && !playerWasAttacked)
        {
            agent.SetDestination(player.position);
            yield return null;
        }
    }

    protected override void AttackPlayer()
    {
        GameManager.instance.currentPlayer.life -= 1;

        PlayerController.instance.isTraining = false;

        PlayerController.instance.isMoving = false;

        PlayerController.instance.isReadyToJump = false;

        PlayerController.instance.StartPosition();

        playerWasAttacked = true;

        Debug.Log("Player attacked!");
    }
}
