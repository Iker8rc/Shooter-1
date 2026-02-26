using UnityEngine;

public class MultiBossController : MultiEnemy
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

            if (player != null && player.photonView.IsMine)
            {
                player.TakeDamage2(damage);
                player.Slow(slowSpeed, slowDuration);
            }
        }
    }
}
