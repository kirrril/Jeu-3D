using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentControllerYouWin : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    private Animator animator;

    [SerializeField]
    AudioSource voiceSweet;
    
    public GameObject[] actionPoints;
    public Coroutine currentCoroutine;
    public string currentCoroutineName;
    int lastIndex = -1;
    float distance;
    public bool isBusy;
    bool playerIsHere;
    public float chaseDistance = 0.2f; // Distance à laquelle elles s'arrêtent près du joueur

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }

    // void Start()
    // {
    //     currentCoroutine = StartCoroutine(MoveToTarget());
    //     currentCoroutineName = "MoveToTarget";
    // }

    void Update()
    {
        UpdateAgentBehaviour();
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        float speed = new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude;
        animator.SetFloat("MovementSpeed", speed > 0.1f ? 1.9f : 0f);
    }

    void UpdateAgentBehaviour()
    {
        distance = Vector3.Distance(player.position, transform.position);
        playerIsHere = distance < 5f;

        if (!isBusy)
        {
            if (playerIsHere)
            {
                if (currentCoroutineName != "FollowPlayer")
                {
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }
                    currentCoroutine = StartCoroutine(FollowPlayer());
                    currentCoroutineName = "FollowPlayer";
                }
            }
            else
            {
                if (currentCoroutineName != "MoveToTarget" || currentCoroutine == null)
                {
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }
                    currentCoroutine = StartCoroutine(MoveToTarget());
                    currentCoroutineName = "MoveToTarget";
                }
            }
        }
    }

    protected virtual IEnumerator FollowPlayer()
    {
        while (true) // Boucle pour suivre en continu
        {
            // Calculer la direction et la distance vers le joueur
            Vector3 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            // Si trop loin, avancer vers le joueur
            if (distanceToPlayer > chaseDistance)
            {
                agent.SetDestination(player.position);

                voiceSweet.Play();
            }
            else
            {
                agent.SetDestination(transform.position); // Arrêter le mouvement
            }

            // Tourner vers le joueur
            Vector3 lookDirection = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;
            if (lookDirection.sqrMagnitude > 0.01f) // Éviter les rotations inutiles si trop près
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            yield return null; // Attendre la frame suivante
        }
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

    void OnTriggerEnter(Collider other)
    {
        // Ignorer complètement les autres "Girl"
        if (this.CompareTag("Girl") && other.CompareTag("Girl"))
        {
            return; // Ne rien faire, laisser l'agent continuer son comportement
        }

        // if (other.CompareTag("Player"))
        // {
        //     voiceSweet.Play();
        // }
    }
}