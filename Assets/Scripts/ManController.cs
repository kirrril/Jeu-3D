using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ManController : AgentController
{
    public bool playerWasAttacked;

    protected override IEnumerator ChaseFleePlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > 1f && !playerWasAttacked)
        {
            if (agent == null || !agent.enabled || !agent.isOnNavMesh)
            {
                yield break;
            }
            
            // Vérifier si le chemin est atteignable
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(player.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                yield break;
            }

            yield return null;
        }
    }
}