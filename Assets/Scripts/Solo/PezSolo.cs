using UnityEngine;

public class PezSolo : SoloEnemyController
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
            SoloPlayerController player = collision.gameObject.GetComponent<SoloPlayerController>();
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (player != null)
            {
                player.TakeDamage(damage);

                if (rb != null)
                {
                    Vector3 direction = (collision.transform.position - transform.position).normalized;
                    direction = new Vector3(direction.x, 0, direction.z);
                    rb.AddForce(direction * knockBackForce);
                }
            }
            touchTimer = touchCooldown;
        }
    }
}
