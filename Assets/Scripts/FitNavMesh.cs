using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FitNavMesh : MonoBehaviour
{
    [SerializeField]
    private Transform player;  // Référence au Player
    [SerializeField]
    private float fleeDistance = 5f; // Distance à laquelle l’agent doit fuir
    [SerializeField]
    private float randomMoveRadius = 10f; // Distance pour un déplacement aléatoire
    
    private NavMeshAgent fitWomanAgent;

    void Start()
    {
        fitWomanAgent = GetComponent<NavMeshAgent>();
    }

    // void Update()
    // {
    //     Vector3 fleeDirection = transform.position - player.position;
    //     fleeDirection = fleeDirection.normalized * fleeDistance;

    //     Vector3 fleePosition = transform.position + fleeDirection;

    //     fitWomanAgent.SetDestination(fleePosition);
    // }

        void Update()
    {
        Vector3 fleeDirection = transform.position - player.position;
        fleeDirection = fleeDirection.normalized * fleeDistance;

        Vector3 fleePosition = transform.position + fleeDirection;

        // Vérifier si la position est sur le NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleePosition, out hit, 5f, NavMesh.AllAreas))
        {
            fitWomanAgent.SetDestination(fleePosition);
        }
        else
        {
            // Si impossible de fuir directement, on génère un mouvement aléatoire
            Vector3 randomDirection = Random.insideUnitSphere * randomMoveRadius;
            randomDirection += transform.position;
            
            if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
            {
                fitWomanAgent.SetDestination(hit.position);
            }
        }
    }
}
