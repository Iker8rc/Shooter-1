using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
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
    private float bulletDamage;
    public bool isDead = false;
    public int totalKills;

    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float shootCooldown;
    private float timePass;
    
    bool ejemplo;
    private Animator animator;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true)
        {
            stream.SendNext(ejemplo);
            stream.SendNext(life);
        }
        else
        {
            ejemplo = (bool)stream.ReceiveNext();
            life = (float)stream.ReceiveNext();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        if(photonView.IsMine == true)
        {
            Camera.main.GetComponent<CamMultiplayerController>().SetPlayer(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timePass += Time.deltaTime; 

        if(photonView.IsMine == true)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 arriba = Vector3.forward + Vector3.left;
            Vector3 derecha = Vector3.forward + Vector3.right;
            Vector3 movement = ((Vector3.right * leftStickInput.y) + (Vector3.back * leftStickInput.x)) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            bool run = leftStickInput.magnitude > 0.1f;
            animator.SetBool("Run", run);
        }
        //mirar
        float y = Camera.main.GetComponent <CamMultiplayerController>().camOffset.y;
        Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
        Vector3 playerRot = transform.eulerAngles; 
        transform.LookAt(worldPos);
        transform.eulerAngles = new Vector3(playerRot.x, transform.eulerAngles.y, playerRot.z);

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
                if (timePass>= shootCooldown)
                {   
                    timePass = 0;
                    GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
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
                    photonView.RPC("CopyShoot", RpcTarget.Others);
                }
            }
                 
        }
    }

    [PunRPC]
    void CopyShoot()
    {
        GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
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
    public void TakeDamage(float _damage, Player player)
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
    }
    public void TakeDamage2(float damage2)
    {
        if (!photonView.IsMine) 
        {
            return; 
        }

        life -= damage2;
        FindAnyObjectByType<MultiLevelManager>().UpdateLife();

        if (life <= 0)
        {
            animator.SetTrigger("Death");
            isDead = true;
            rb.linearVelocity = Vector3.zero; 
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }
    void VerMuertes()
    {
        for(int i = 0; i <PhotonNetwork.CurrentRoom.PlayerCount; i ++)
        {
            //PhotonNetwork.CurrentRoom.Players[i].CustomProperties.TryGetValue("Muertes", out nombrevariable);
        }
        
    }
}
