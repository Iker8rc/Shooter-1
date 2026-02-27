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
        player.life +=1;
        FindObjectOfType<SoloLevelManager>().UpdateLife();
        Destroy(gameObject);
    }
}
