using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerInput playerInput;
    [SerializeField]
    private float speed;
    private Rigidbody rb;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if(photonView.IsMine == true)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 movement = (transform.forward * leftStickInput.y + transform.right * leftStickInput.x) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        }
        
    }
}
