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
}
