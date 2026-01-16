using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;
    private Rigidbody rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lookSpeed;
    [SerializeField]
    private Transform followTarget;
    private LevelManager levelManager;
    [SerializeField]
    private float timeToStartHealth;
    [SerializeField]
    private float healthSpeed;
    private IEnumerator corrutineCurar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        animator.SetFloat("Horizontal", leftStickInput.x);
        animator.SetFloat("Vertical", leftStickInput.y);
        Vector3 movement = (transform.forward * leftStickInput.y + transform.right * leftStickInput.x) * speed;
        rb.linearVelocity = new Vector3 (movement.x, rb.linearVelocity.y, movement.z);
    }
    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        followTarget.localEulerAngles += new Vector3(lookInput.y * lookSpeed * Time.deltaTime, 0, 0);
        transform.eulerAngles += new Vector3(0, lookInput.x * lookSpeed * Time.deltaTime, 0); 
    }
    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            animator.SetBool("Shooting", true);
            GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].Triggered();
            playerInput.actions["Reload"].Disable();
        }
        else if (callbackContext.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("Shooting", false);
            GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].TriggerReleased();
            playerInput.actions["Reload"].Enable();
        }
    }
    public void Reload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            //anim recarga
            animator.SetTrigger("Reload");
            GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].Reload();
            levelManager.UpdateBullets();
            playerInput.actions["Shoot"].Disable();
        }
    }
    public void CanShoot()
    {
        playerInput.actions["Shoot"].Enable();
    }
    public void TakeDamage(float _damage)
    {
        if(corrutineCurar != null)
        {
            StopCoroutine(corrutineCurar);
        }
       
        GameManager.instance.GetGameData.CurrentLife -= _damage;
        if(GameManager.instance.GetGameData.CurrentLife <= 0)
        {
            //Death
            GameObject ragdollPrefab = (GameObject) Resources.Load("SwatRagdoll");
            Instantiate(ragdollPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else
        {
            corrutineCurar = Health();
            StartCoroutine(corrutineCurar);
        }
        levelManager.UpdateLife();
    }
    IEnumerator Health()
    {
        yield return new WaitForSeconds(timeToStartHealth);
        while (GameManager.instance.GetGameData.CurrentLife < GameManager.instance.GetGameData.MaxLife)
        {
            GameManager.instance.GetGameData.CurrentLife = Mathf.Clamp(GameManager.instance.GetGameData.CurrentLife + (healthSpeed*Time.deltaTime), 0, 
                GameManager.instance.GetGameData.MaxLife);
            levelManager.UpdateLife();
            yield return null;
        }

    }
}
