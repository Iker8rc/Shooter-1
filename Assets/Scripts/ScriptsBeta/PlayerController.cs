using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField]
    private Transform rightHand, leftHand;
    [SerializeField]
    private GameObject grenadePrefab;
    [SerializeField]
    private Transform grenadeSpawnPoint;
    private LineRenderer lineRenderer;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private Transform spineBone;
    [SerializeField]
    private float spineOffset;

    private 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        lineRenderer = grenadeSpawnPoint.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        animator.SetFloat("Horizontal", leftStickInput.x);
        animator.SetFloat("Vertical", leftStickInput.y);
        Vector3 movement = (transform.forward * leftStickInput.y + transform.right * leftStickInput.x) * speed;
        rb.linearVelocity = new Vector3 (movement.x, rb.linearVelocity.y, movement.z);

        //Line renderer grenade
        if(lineRenderer.enabled == true)
        {
            Vector3 speed = (Camera.main.transform.forward + Vector3.up) * throwForce;
            lineRenderer.positionCount = 100;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                float t /*t de tiempo*/ = i * 0.1f;
                Vector3 position = grenadeSpawnPoint.position + speed * t + 0.5f * Physics.gravity * t * t;
                lineRenderer.SetPosition(i, position);
            }
        }
    }
    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        followTarget.localEulerAngles += new Vector3(lookInput.y * lookSpeed * Time.deltaTime, 0, 0);
        transform.eulerAngles += new Vector3(0, lookInput.x * lookSpeed * Time.deltaTime, 0);
        spineBone.localEulerAngles = new Vector3(followTarget.localEulerAngles.x + spineOffset, spineBone.localEulerAngles.y, spineBone.localEulerAngles.z);
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
            playerInput.actions["ThrowGrenade"].Disable();
        }
    }
    public void CanShoot()
    {
        playerInput.actions["Shoot"].Enable();
        playerInput.actions["ThrowGrenade"].Enable();
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
    public void ThrowGrenade (InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetBool("Grenade", true);

            //Cambiar arma
            GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].transform.parent = leftHand;

            //Granada
            Instantiate(grenadePrefab, grenadeSpawnPoint.position, grenadeSpawnPoint.rotation,grenadeSpawnPoint);

            //Mostrar linea trayectoria
            lineRenderer.enabled = true;

            //Desactivar que se pueda disparar
            playerInput.actions["Shoot"].Disable();
        }

        if (context.canceled)
        {
            animator.SetBool("Grenade", false);
            lineRenderer.enabled = false;
        }
    }
    public void SoltarGranada()
    {
        Transform grenadeClone= grenadeSpawnPoint.GetChild(0);
        grenadeClone.parent = null;
        grenadeClone.GetComponent<Rigidbody>().isKinematic = false;
        grenadeClone.GetComponent<Rigidbody>().linearVelocity = (Camera.main.transform.forward + Vector3.up) * throwForce;
        grenadeClone.GetComponent<Collider>().enabled = true;
        grenadeClone.GetComponent <Grenade>().countDownActive = true;
    }
    public void FinishGrenade()
    {
        CanShoot();
        GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].transform.parent = rightHand;
    }
}
