using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using ExitGames.Client.Photon;

public class MultiEnemy : MonoBehaviourPunCallbacks, IPunObservable
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

    private MultiLevelManager levelManager;

    [SerializeField]
    private bool pez;

    //Audio
    [SerializeField]
    private AudioClip roar;
    [SerializeField]
    private AudioClip death;

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
        levelManager = FindObjectOfType<MultiLevelManager>();
    }

    public void Update()
    {
        if (Muerto == true)
        {
            return;
        }
        if (targetPlayer.GetComponent<MultiplayerController>().isDead == true)
        {
            agent.isStopped = true;
            animator.SetBool("Iddle", true);
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
    public void TakeDamage(float _damage, Player _owner)
    {
        Debug.Log("Recibe daño");
        life -= _damage;

        if (life <= 0)
        {
            Die(_owner);
        }
    }

    public void Die(Player _owner)
    {
        //Desactivar followPlayer
        agent.isStopped = true;
        Muerto = true;
        animator.SetTrigger("Death");
        if (pez == true)
        {
            transform.localPosition += new Vector3(1.9f, 0, 0);
        }
        GetComponent<Collider>().enabled = false;
        
        Destroy(gameObject, 2f);
        
        if (_owner != null)
        {
            foreach (var player in FindObjectsOfType<MultiplayerController>())
            {
                if (player.photonView.Owner == _owner) 
                {
                    Debug.Log("JugadorLoMata");
                    player.GlobalKills();
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true)
        {
            stream.SendNext(life);
        }
        else
        {
            life = (float)stream.ReceiveNext();
        }
    }
}

