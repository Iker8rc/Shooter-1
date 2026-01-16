using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

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
    [SerializeField]
    private float life;
    [SerializeField]
    private Weapon weapon;
    private bool reloading;

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
            float distance = (player.position - transform.position).magnitude;
            if (distance <= 10)
            {
                //Disparar
                animator.SetFloat("Vertical", 0);
                transform.LookAt(player);
                if(reloading == false)
                {
                    weapon.EnemyShoot(player);
                }
            }
        }
        else
        {
            if (patrolPoints.Length > 0)
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
    public void TakeDamage(float _damage)
    {
        life-= _damage;
        following = true;
        if(life <=0)
        {
            //muerto
            GameObject ragdollPrefab = (GameObject) Resources.Load("EnemyRagdoll");
            Instantiate(ragdollPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else
        {
            //vivo + hit reaction
            animator.SetTrigger("Hit");
        }
    }
    public void Reload()
    {
        reloading = true;
        animator.SetTrigger("Reload");
        weapon.Reload();
    }

    public void FinishReload()
    {
        reloading = false; 
    }
}
