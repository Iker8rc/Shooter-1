using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;



public class MultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private float speed;
    private Rigidbody rb;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    public float life;
    [SerializeField]
    public float maxLife;
    [SerializeField] 
    private float bulletDamage;
    public bool isDead = false;
    public int totalKills;

    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float shootCooldown;
    private float timePass;
    public bool isPaused = false;
    
    bool ejemplo;
    private Animator animator;
    private float originalSpeed;
    private bool isSlowed = false;
    public MultiLevelManager levelManager;

    //Audio
    [SerializeField]
    private AudioClip shoot;
    [SerializeField]
    private AudioClip muerte;
    [SerializeField]
    private AudioClip walk;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true)
        {
            stream.SendNext(ejemplo);
            stream.SendNext(life);
            stream.SendNext(isDead);

        }
        else
        {
            ejemplo = (bool)stream.ReceiveNext();
            life = (float)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        
        levelManager = FindObjectOfType<MultiLevelManager>();

        originalSpeed = speed;
        //Invoke("CamSet", 1);    
    }

    private void CamSet()
    {
        if (photonView.IsMine == true)
        {
            levelManager.player = this;
            Camera.main.GetComponent<CamMultiplayerController>().SetPlayer(transform);
        }
    }
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || isDead) 
        {
            return;   
        }
        timePass += Time.deltaTime; 

        if(photonView.IsMine == true)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 arriba = Vector3.forward + Vector3.left;
            Vector3 derecha = Vector3.forward + Vector3.right;
            Vector3 movement = ((Vector3.right * leftStickInput.y) + (Vector3.back * leftStickInput.x)) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
            //mirar
            float y = Camera.main.GetComponent <CamMultiplayerController>().camOffset.y;
            Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
            Vector3 playerRot = transform.eulerAngles; 
            transform.LookAt(worldPos);
            transform.eulerAngles = new Vector3(playerRot.x, transform.eulerAngles.y, playerRot.z);
            bool run = leftStickInput.magnitude > 0.1f;
            
            animator.SetBool("Run", run);
        }     

    }

    /// <summary>
    /// Opcion 1 de disparo online
    /// Llama a un metodo en el resto de copias del usuario
    /// </summary>
    /// <param name="context"></param>

    public void Shoot(InputAction.CallbackContext context)
    {
        if(photonView.IsMine==true)
        {
            if(context.performed == true)
            {
                if (life <= 0 || isDead) 
                {
                    return;
                }
                if (timePass>= shootCooldown)
                {   
                    //AudioManager.instance.PlaySFX(shoot, transform.position);
                    timePass = 0;
                    GameObject bulletClone = PhotonNetwork.Instantiate("MultiBullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    Rigidbody rbBullet = bulletClone.GetComponent<Rigidbody>();
                    if (rbBullet != null)
                    {
                        rbBullet.linearVelocity = bulletClone.transform.forward * bulletSpeed;               
                    }
                
                    MultiBullet bulletScript = bulletClone.GetComponent<MultiBullet>();
                    if (bulletScript != null)
                    {
                        bulletScript.damage = bulletDamage;
                        Debug.Log("BalaDisparao");
                        bulletScript.owner = photonView.Owner;
                    }
                }
            }
                 
        }
    }

    [PunRPC]
    void CopyShoot()
    {
        GameObject bulletClone = PhotonNetwork.Instantiate("MultiBullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation);        
        Rigidbody rbBullet = bulletClone.GetComponent<Rigidbody>();
        if (rbBullet != null)
        {
            rbBullet.linearVelocity = bulletClone.transform.forward * bulletSpeed;
        }
            
        MultiBullet bulletScript = bulletClone.GetComponent<MultiBullet>();
        if (bulletScript != null)
        {
            bulletScript.damage = bulletDamage;
        }
    }
    /// <summary>
    /// Opcion 2 de disparo online
    /// Que se sincronice la bala en todos lados
    /// </summary>
    /// <param name="context"></param>
    void Shoot2(InputAction.CallbackContext context)
    {
        if(photonView.IsMine && context.performed)
        {
            GameObject bulletClone = PhotonNetwork.Instantiate("MultiBullet", bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody rbBullet = bulletClone.GetComponent<Rigidbody>();
        
            if(rbBullet != null)
            {
                rbBullet.linearVelocity = bulletClone.transform.forward * 10; 
            }
                MultiBullet bulletScript = bulletClone.GetComponent<MultiBullet>();       

            if(bulletScript != null)
            {
                bulletScript.damage = bulletDamage;
                bulletScript.owner = photonView.Owner;
            }
        }              
    }
    //Esto en realidad va en la bala //
    void OnCollisionEnter(Collision collision)
    {
        if(photonView.IsMine == true)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<MultiEnemy>().TakeDamage(10, photonView.Owner);
            }
        }
    }
    
    //En el script del enemigo //
    
    /*public void TakeDamage(float _damage, Player player)
    {
        life -= _damage;
        if (life <= 0)
        {
            animator.SetTrigger("Death");
            int deaths = 0;
            if(player.CustomProperties.ContainsKey("Muertes")== true)
            {
                object muertes;
                player.CustomProperties.TryGetValue("Muertes", out muertes);
                deaths = (int)muertes;
                deaths += 1;        
            }
            else
            {
               deaths = 1;

            }
            Hashtable muerdeths = new Hashtable { { "Muertes", deaths } };
            player.SetCustomProperties(muerdeths);
        }
        
        else
        {
            animator.SetTrigger("Death");
        }  
    } */
    public void TakeDamage2(float damage2)
    {
        if (photonView.IsMine == false)
        {
            return;
        }

        life -= damage2;
        FindAnyObjectByType<MultiLevelManager>().UpdateLife();

        if (life <= 0)
        {
            //AudioManager.instance.PlaySFX(muerte, transform.position);
            animator.SetTrigger("Death");
            isDead = true;
            MainMenu gameManager = FindAnyObjectByType<MainMenu>();
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            
            if (levelManager.TodosMuertos())
            {
                FindAnyObjectByType<MainMenu>().TodosMuertos();
            }
            else
            {
                StartCoroutine(levelManager.RespawnCountdown(this));
            }
        }
    }
    public void GlobalKills()
    {
        totalKills += 1;
        Hashtable kills = new Hashtable
        {
            {"Kills", totalKills }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(kills);
        FindAnyObjectByType<MultiLevelManager>().UpdateKills();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Kills") == true)
        {
            //Panel final
        }
    }

    public void Slow(float newSpeed, float duration) // Esto es del boss
    {
        if (photonView.IsMine == false) 
        {
            return;
        }

        if (isSlowed == false)
        {

            StartCoroutine(SlowEffect(newSpeed, duration));
        }
    }
    private System.Collections.IEnumerator SlowEffect(float newSpeed, float duration)
    {
        isSlowed = true;
        speed = newSpeed;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        isSlowed = false;
    }
    void VerMuertes()
    {
        for(int i = 0; i <PhotonNetwork.CurrentRoom.PlayerCount; i ++)
        {
            //PhotonNetwork.CurrentRoom.Players[i].CustomProperties.TryGetValue("Muertes", out nombrevariable);
        }       
    }
}
