using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class GirlController : MonoBehaviour
{
    public static GirlController instance;

    [SerializeField]
    NavMeshAgent agent;

    public GameObject[] actionPoints;

    // private float movingSpeed = 3.0f;

    private Rigidbody rb;

    public bool isBusy;

    public bool isMoving;

    Coroutine actifCorout;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        MoveToTarget();
    }



    void MoveToTarget()
    {
        actifCorout = StartCoroutine(MoveToTargetCorout());
    
    }


    IEnumerator MoveToTargetCorout()
    {
        int targetIndex = Random.Range(0, actionPoints.Length);

        Vector3 targetPosition = actionPoints[targetIndex].transform.position;

        agent.SetDestination(targetPosition);

        yield return new WaitForSeconds(4);

        while (agent.remainingDistance > 1f)
        {
            isMoving = true;
            yield return null;
        }

        StartAction(actionPoints[targetIndex]);

        actifCorout = null;
    }

    void StartAction(GameObject actionPoint)
    {
        IInteractable iInteractable = actionPoint.GetComponent<IInteractable>();

        iInteractable.Interact(this.gameObject, MoveToTarget);
    }


    void StopPatrol()
    {
        agent.isStopped = true;

        if (actifCorout != null)
        {
            StopCoroutine(actifCorout);

            actifCorout = null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopPatrol();

        }
    }
}