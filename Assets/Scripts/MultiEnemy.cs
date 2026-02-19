using UnityEngine;

public class MultiEnemy : MonoBehaviour
{
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float life = 100f;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float attackDamage = 10f;
    [SerializeField]
    private float attackCooldown = 1.2f;

    private Transform targetPlayer;  
    private float attackTimer = 0f;

    private Transform FollowPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform follow = null;
        float minDistance = Mathf.Infinity;


    foreach (GameObject player in players)
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < minDistance)
        {
            minDistance = dist;
            follow = player.transform;
        }
    }

    return follow;
}
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = FollowPlayer();
    }

    void Update()
    {
        if (targetPlayer == null)
        {
            targetPlayer = FollowPlayer();
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

        animator.SetTrigger("Attack"); 
        attackTimer = attackCooldown;
        Damage();
    }

    private void Damage()
    {
        if (targetPlayer == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        if (distance > attackRange) 
        {
            return;
        }

        Debug.Log("HaceDaño");

        /*PlayerHealth playerHealth = targetPlayer.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }*/
    }
    public void TakeDamage(float damage)
    {
        // Reducir vida
        // Si llega a 0, morir
        // Activar animación de hit (si quieres)
    }

    private void Die()
    {
        // Animación de muerte o desactivar enemigo
    }
}

