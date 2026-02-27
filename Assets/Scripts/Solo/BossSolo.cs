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
            MultiplayerController player = collision.gameObject.GetComponent<MultiplayerController>();

            if (player != null)
            {
                player.TakeDamage2(damage);
                player.Slow(slowSpeed, slowDuration);
            }
        }
    }
}
