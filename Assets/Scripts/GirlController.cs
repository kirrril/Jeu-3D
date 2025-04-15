using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GirlController : AgentController
{
    private float fleeDistance = 2f;

    [SerializeField] private AudioSource voiceAgressive;
    [SerializeField] private AudioSource voiceSweet;

    protected override IEnumerator ChaseFleePlayer()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 fleeDirection = (transform.position - player.position).normalized * fleeDistance;
        Vector3 fleePosition = transform.position + fleeDirection;

        // Trouver une position valide sur le NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, 2f, NavMesh.AllAreas))
        {
            fleePosition = hit.position;

            if (agent.enabled && !agent.isStopped) ///////////////////
            {
                agent.SetDestination(fleePosition);
            }
        }
        else
        {
            Debug.LogWarning($"Position de fuite non valide pour {gameObject.name}: {fleePosition}");
        }

        if (voiceAgressive != null)
        {
            voiceAgressive.Play();
        }
    }
}