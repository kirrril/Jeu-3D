using System.Collections;
using UnityEngine;


public class GirlPatrol : MonoBehaviour
{
    // public static GirlPatrol instance;

    // public Transform[] patrolPoints;

    // private int targetIndex;

    // private float movingSpeed = 3.0f;

    // private Rigidbody rb;

    // public bool isBusy;

    // public bool isMoving;


    // void Awake()
    // {
    //     instance = this;
    // }

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();

    //     StartPosition();

    //     targetIndex = Random.Range(0, patrolPoints.Length);
    // }


    // void FixedUpdate()
    // {
    //     MoveToTarget();
    // }


    // void StartPosition()
    // {
    //     transform.position = new Vector3(8, 0, -2);
    // }


    // void MoveToTarget()
    // {
    //     Vector3 targetPosition = patrolPoints[targetIndex].position;

    //     if (Vector3.Distance(rb.position, targetPosition) > 0.2f)
    //     {
    //         RotateTowards(targetPosition);

    //         rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, movingSpeed * Time.fixedDeltaTime));

    //         isMoving = true;

    //     }

    //     if (Vector3.Distance(rb.position, targetPosition) < 0.2f)
    //     {
    //         isMoving = false;

    //         isBusy = true;

    //         return;
    //     }
    // }

    // void RotateTowards(Vector3 target)
    // {
    //     Vector3 direction = (target - transform.position).normalized;

    //     if (direction != Vector3.zero)
    //     {
    //         transform.rotation = Quaternion.LookRotation(direction);
    //     }
    // }


    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Training"))
    //     {
    //         StartCoroutine(WaitWhileBusy());
    //     }

    //     isBusy = false;
    // }


    // IEnumerator WaitWhileBusy()
    // {
    //     yield return new WaitForSeconds(10);

    //     isBusy = false;

    //     targetIndex = Random.Range(0, patrolPoints.Length);

    //     MoveToTarget();
    // }
}