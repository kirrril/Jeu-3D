using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class AgentController : MonoBehaviour
{
    public static AgentController instance;

    [SerializeField]
    NavMeshAgent agent;

    private Animator animator;

    public GameObject[] actionPoints;

    int lastIndex = -1;


    void Awake()
    {
        instance = this;

        animator = GetComponentInChildren<Animator>();
    }


    void Start()
    {
        StartCoroutine(MoveToTarget());
    }


    public IEnumerator MoveToTarget()
    {
        yield return new WaitForSeconds(0.2f);

        int targetIndex;

        do
        {
            targetIndex = Random.Range(0, actionPoints.Length);

        } while (targetIndex == lastIndex);

        lastIndex = targetIndex;

        Vector3 targetPosition = actionPoints[targetIndex].transform.position;

        animator.SetFloat("MovementSpeed", 1.9f);

        agent.SetDestination(targetPosition);
    }
}