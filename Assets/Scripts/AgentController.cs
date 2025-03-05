using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class AgentController : MonoBehaviour
{
    public static AgentController instance;

    protected NavMeshAgent agent;

    protected Transform player;

    private Animator animator;

    public GameObject[] actionPoints;

    int lastIndex = -1;

    float distance;

    bool playerIsHere;


    void Awake()
    {
        instance = this;

        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").transform;

        animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        CheckPlayer();
    }


    void CheckPlayer()
    {
        distance = Vector3.Distance(player.position, transform.position);

        if (distance < 4f)
        {
            playerIsHere = true;
        }

        if (distance > 4f)
        {
            playerIsHere = false;
        }


        if (!PlayerController.instance.isTraining && !PlayerController.instance.isReadyToJump && playerIsHere)
        {
            StartCoroutine(ChaseFleePlayer());
        }

        if (PlayerController.instance.isTraining || PlayerController.instance.isReadyToJump || !playerIsHere)
        {
            StartCoroutine(MoveToTarget());
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

        animator.SetFloat("MovementSpeed", 1.9f);

        agent.SetDestination(targetPosition);
    }


    void InteractWithPlayer()
    {
        AttackPlayer();

        Debug.Log("InteractWithPlayer called!");
    }


    void AgentsChatting()
    {
        Debug.Log("Blah Blah Blah Blah");
    }


    protected virtual void AttackPlayer()
    {
        Debug.Log("AttackPlayer called!");
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            InteractWithPlayer();

            Debug.Log("Collision detected!");
        }


        if (collision.gameObject.CompareTag("Agent"))
        {
            AgentsChatting();

            Debug.Log("Agents are chatting!");
        }
    }
}