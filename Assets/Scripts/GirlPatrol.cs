using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class GirlPatrol : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent agent;

    public static GirlPatrol instance;

    public GameObject[] actionPoints;

    private int targetIndex;

    private float movingSpeed = 3.0f;

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
            yield return null;
        }

        StartAction(actionPoints[targetIndex]);

        actifCorout = null;
    }

    void StartAction(GameObject actionPoint)
    {
        IInteractable iinteractable = actionPoint.GetComponent<IInteractable>();

        iinteractable.Interact(this.gameObject, MoveToTarget);
    }


    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
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

    IEnumerator WaitWhileBusy()
    {
        yield return new WaitForSeconds(10);

        isBusy = false;

        targetIndex = Random.Range(0, actionPoints.Length);

        MoveToTargetCorout();
    }
}