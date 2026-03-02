using UnityEngine;
using UnityEngine.InputSystem;

public class SoloPlayerController : MonoBehaviour
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

    //Audio
    [SerializeField]
    private AudioClip shoot;
    [SerializeField]
    private AudioClip muerte;
    [SerializeField]
    private AudioClip walk;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
        Camera.main.GetComponent<CamMultiplayerController>().SetPlayer(transform);  
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            return;
        }
        timePass += Time.deltaTime;

        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 arriba = Vector3.forward + Vector3.left;
        Vector3 derecha = Vector3.forward + Vector3.right;
        Vector3 movement = ((Vector3.right * leftStickInput.y) + (Vector3.back * leftStickInput.x)) * speed;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

        bool run = leftStickInput.magnitude > 0.1f;
        animator.SetBool("Run", run);
        
        
        //mirar
        float y = Camera.main.GetComponent<CamMultiplayerController>().camOffset.y;
        Vector2 mousePos = playerInput.actions["LookCenital"].ReadValue<Vector2>();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, y));
        Vector3 playerRot = transform.eulerAngles;
        transform.LookAt(worldPos);
        transform.eulerAngles = new Vector3(playerRot.x, transform.eulerAngles.y, playerRot.z);

    }

    public void Shoot(InputAction.CallbackContext context)
    {
            if (context.performed == true)
            {
                if (life <= 0) 
                {
                    return;
                }
                if (timePass >= shootCooldown)
                {
                    timePass = 0;
                    AudioManager.instance.PlaySFX(shoot, transform.position);
                    GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                    Rigidbody rbBullet = bulletClone.GetComponent<Rigidbody>();
                    if (rbBullet != null)
                    {
                        rbBullet.linearVelocity = bulletClone.transform.forward * bulletSpeed;

                    }

                    SoloBullet bulletScript = bulletClone.GetComponent<SoloBullet>();
                    if (bulletScript != null)
                    {
                        bulletScript.damage = bulletDamage;
                    }
                 
                }
            }
    }

    public void TakeDamage(float damage)
    {
   
        life -= damage;
        FindAnyObjectByType<SoloLevelManager>().UpdateLife();

        if (life <= 0)
        {
            AudioManager.instance.PlaySFX(muerte, transform.position);
            animator.SetTrigger("Death");
            isDead = true;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
            FindAnyObjectByType<SoloMainMenu>().GameOver();
        }
    }

    public void Slow(float newSpeed, float duration) // Esto es del bosssss
    {
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
}
