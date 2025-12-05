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
    [SerializeField]
    private Weapon weapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
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
        weapon.Shoot();

        if (callbackContext.phase == InputActionPhase.Started)
        {
            animator.SetBool("Shooting", true);
        }
        else if (callbackContext.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("Shooting", false);
        }
    }
    public void Reload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            //anim recarga
            weapon.Reload();
        }
    }
}
