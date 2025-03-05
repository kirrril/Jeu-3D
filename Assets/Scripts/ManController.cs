using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ManController : AgentController
{
    protected override IEnumerator ChaseFleePlayer()
    {
        yield return new WaitForSeconds(0.5f);

        agent.SetDestination(player.position);

        yield return new WaitForSeconds(0.5f);
    }

    protected override void AttackPlayer()
    {
        GameManager.instance.currentPlayer.life -= 1;

        PlayerController.instance.isTraining = false;

        PlayerController.instance.isMoving = false;

        PlayerController.instance.isReadyToJump = false;

        PlayerController.instance.StartPosition();

        Debug.Log("Player attacked!");
    }
}
