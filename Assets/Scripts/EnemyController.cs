using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private float speed;
    private Transform player;
    private NavMeshAgent agent;
    private bool following;
    [SerializeField]
    private Transform[] patrolPoints;
    private int patrolIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (following == true)
        {
            agent.speed = speed;
            agent.stoppingDistance = 10;
            animator.SetFloat("Vertical", 1f);
            agent.SetDestination(player.position);
        }
        else
        {
            agent.speed = speed * 0.5f;
            animator.SetFloat("Vertical", 0.4f);
            agent.SetDestination(patrolPoints[patrolIndex].position);
            float distance = (patrolPoints[patrolIndex].position - transform.position).magnitude;
            if (distance < 1)
            {
                patrolIndex += 1;
                if (patrolIndex >= patrolPoints.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 1.65f, 0), (player.position - transform.position).normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    following = true; 
                }
            }
        }
    }
}
