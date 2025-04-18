using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    private Animator animator;
    public GameObject[] actionPoints;

    public Coroutine currentCoroutine;
    public string currentCoroutineName;
    private int lastIndex = -1;
    private float distance;
    public bool isBusy;
    private bool playerIsHere;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        StartMoveToTarget();
    }

    void Update()
    {
        UpdateAgentBehaviour();
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        if (!agent.enabled) return;

        float speed = new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude;
        if (speed > 0.005f)
        {
            animator.SetFloat("MovementSpeed", 1.9f);
        }
        else
        {
            animator.SetFloat("MovementSpeed", 0f);
        }
    }

    void UpdateAgentBehaviour()
    {
        if (isBusy)
        {
            return;
        }

        Vector3 playerPosFlat = new Vector3(player.position.x, 0, player.position.z);
        Vector3 agentPosFlat = new Vector3(transform.position.x, 0, transform.position.z);
        distance = Vector3.Distance(playerPosFlat, agentPosFlat);

        playerIsHere = distance < 3f;

        if (!PlayerController.instance.isTraining && !PlayerController.instance.isReadyToJump && playerIsHere)
        {
            if (currentCoroutineName != "ChaseFleePlayer")
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }

                currentCoroutine = StartCoroutine(ChaseFleePlayer());
                currentCoroutineName = "ChaseFleePlayer";
            }
        }
        else
        {
            if (currentCoroutineName != "MoveToTarget" || currentCoroutine == null)
            {
                StartMoveToTarget();
            }
        }
    }

    protected virtual IEnumerator ChaseFleePlayer()
    {
        yield return null;
    }

    public void StartMoveToTarget()
    {
        if (isBusy)
        {
            return;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(MoveToTarget());
        currentCoroutineName = "MoveToTarget";
    }

    public IEnumerator MoveToTarget()
    {
        yield return new WaitForSeconds(0.1f);

        int targetIndex = lastIndex;
        
        while (targetIndex == lastIndex)
        {
            targetIndex = Random.Range(0, actionPoints.Length);
        }

        lastIndex = targetIndex;

        Vector3 targetPosition = actionPoints[targetIndex].transform.position;

        if (agent.enabled && !agent.isStopped)
        {
            agent.SetDestination(targetPosition);
        }

        if (!agent.enabled || agent.isStopped)
        {
            yield break;
        }

        while (agent.enabled && agent.remainingDistance > agent.stoppingDistance && !agent.isStopped)
        {
            yield return null;
        }

        StartMoveToTarget();
    }
}