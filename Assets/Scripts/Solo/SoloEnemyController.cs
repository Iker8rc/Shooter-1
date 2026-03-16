using UnityEngine;
using UnityEngine.InputSystem;

public class SoloEnemyController : MonoBehaviour
{
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float life;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    public float damage;
    [SerializeField]
    private float attackCooldown;
    private Transform targetPlayer;
    private float attackTimer;
    private bool Muerto;
    [SerializeField]
    private bool pez;

    private SoloPlayerController player;
    private SoloLevelManager levelManager;

    //Audio
    [SerializeField]
    private AudioClip roar;
    [SerializeField]
    private AudioClip death;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        player = FindObjectOfType<SoloPlayerController>();
        levelManager = FindObjectOfType<SoloLevelManager>();
    }

    public void Update()
    {
        if (Muerto == true)
        {
            return;
        }
        if (targetPlayer.GetComponent<SoloPlayerController>().isDead == true)
        {
            agent.isStopped = true;
            animator.SetBool("Iddle", true);
            return;
        }

        if (targetPlayer == null)
        {
            targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            if (targetPlayer == null)
            {
                return;
            }
        }

        agent.SetDestination(targetPlayer.position);
        agent.speed = speed;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance <= attackRange)
        {
            
            agent.isStopped = true;
            Attack();
        }
        else
        {
            agent.isStopped = false;
        }

        //cooldown

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (attackTimer > 0)
        {
            return;
        }
        AudioManager.instance.PlaySFX(roar, transform.position);
        animator.SetTrigger("Attack");
        attackTimer = attackCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SoloPlayerController>().TakeDamage(damage);
        }
    }
    public void TakeDamage(float _damage)
    {
        Debug.Log("Recibe da�o");
        life -= _damage;

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.instance.PlaySFX(death, transform.position);
        agent.Stop();
        agent.isStopped = true;
        Muerto = true;
        animator.SetTrigger("Death");
        if (pez == true)
        {
            Debug.Log("Cambia pos");
            transform.localPosition += new Vector3(1.9f, 0, 0);
        }
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);

        player.totalKills++;
        levelManager.UpdateKills();
    }
}

