using UnityEngine;

public class BossSolo : SoloEnemyController
{
    [SerializeField]
    private float slowSpeed;
    [SerializeField]
    private float slowDuration;
    void Update()
    {
        base.Update();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoloPlayerController player = collision.gameObject.GetComponent<SoloPlayerController>();

            if (player != null)
            {
                player.TakeDamage(damage);
                player.Slow(slowSpeed, slowDuration);
            }
        }
    }
}
