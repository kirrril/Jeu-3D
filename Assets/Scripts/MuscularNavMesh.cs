using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MuscularNavMesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public Transform playerPosition;

    public Transform mirrorPosition;

    private Vector3 lastPlayerPosition;
    private Vector3 newPlayerPosition;

    public static bool showMuscles = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        PlaceMuscularMan();

        lastPlayerPosition = playerPosition.position;
    }

    void OnTriggerEnter(Collider other)
    {
        newPlayerPosition = playerPosition.position;

        if (other.CompareTag("Mirror"))
        {
            showMuscles = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        showMuscles = false;
    }

    void PlaceMuscularMan()
    {
        navMeshAgent.destination = playerPosition.position;

        if (showMuscles == true)
        {
            if (Vector3.Distance(lastPlayerPosition, newPlayerPosition) < 0.1f)
            {
                navMeshAgent.SetDestination(mirrorPosition.position);
                navMeshAgent.transform.rotation = mirrorPosition.rotation;
            }
            else if (showMuscles == false)
            {
                navMeshAgent.SetDestination(playerPosition.position);
            }
        }
    }
}
