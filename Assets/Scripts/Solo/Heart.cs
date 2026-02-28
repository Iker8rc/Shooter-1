using UnityEngine;

public class Heart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        SoloPlayerController player = other.GetComponent<SoloPlayerController>();
        if (player == null)
        {
            return;
        }
        if (player.life < player.maxLife)
        {
            player.life += 1;
            FindObjectOfType<SoloLevelManager>().UpdateLife();
            Destroy(gameObject);
        }
        else
        {
            
        }
    }
}
