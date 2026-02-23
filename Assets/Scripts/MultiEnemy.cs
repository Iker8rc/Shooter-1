using UnityEngine;

public class MultiEnemy : MonoBehaviour
{
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float life;
    [SerializeField]
    private float attackRange = 1.5f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float attackCooldown = 1.2f;
    private Transform targetPlayer;  
    private float attackTimer = 0f;
    private bool Muerto;

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
        if (Muerto == true)
        {
            return;
        }

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<MultiplayerController>().TakeDamage2(damage);
        }
    }
    public void TakeDamage(float _damage)
    {
        Debug.Log("Recibe daño");
        life -= _damage;

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Desactivar followPlayer
        agent.Stop();
        agent.isStopped = true;
        Muerto = true;
        animator.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}

