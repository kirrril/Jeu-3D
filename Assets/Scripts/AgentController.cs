using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
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

    int lastIndex = -1;

    float distance;

    public bool isBusy;

    bool playerIsHere;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").transform;

        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        currentCoroutine = StartCoroutine(MoveToTarget());
        currentCoroutineName = "MoveToTarget";
    }

    void Update()
    {
        UpdateAgentBehaviour();
        UpdateSpeed();
    }


    void UpdateSpeed()
    {
        float speed = new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude;

        if (speed > 0.1f)
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
        distance = Vector3.Distance(player.position, transform.position);

        playerIsHere = distance < 3f;

        if (!isBusy)
        {
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
                if (currentCoroutineName != "MoveToTarget" && currentCoroutine != null)
                {
                    currentCoroutine = null;

                    currentCoroutine = StartCoroutine(MoveToTarget());
                    currentCoroutineName = "MoveToTarget";
                }

                if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(MoveToTarget());
                    currentCoroutineName = "MoveToTarget";
                }

            }
        }
    }


    protected virtual IEnumerator ChaseFleePlayer()
    {
        yield return null;
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

        agent.SetDestination(targetPosition);

        yield return null;
    }
}