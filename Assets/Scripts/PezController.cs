using UnityEngine;

public class PezController : MultiEnemy
{
    [SerializeField] 
    private float knockBackForce;
    [SerializeField] 
    private float touchCooldown;
    private float touchTimer; //esto es para evitar 

    void Update()
    {
        base.Update();
        
        if (touchTimer > 0)
        touchTimer -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (touchTimer > 0) 
        {
            return; 
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            MultiplayerController player = collision.gameObject.GetComponent<MultiplayerController>();
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (player != null && player.photonView.IsMine)
            {
                player.TakeDamage2(damage);

                if (rb != null)
                {
                    Vector3 direction = (collision.transform.position - transform.position).normalized;
                    rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
                }
            }
            touchTimer = touchCooldown;  
        }
    }
}
