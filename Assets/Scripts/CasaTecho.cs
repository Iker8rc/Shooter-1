using UnityEngine;

public class CasaTecho : MonoBehaviour
{
    [SerializeField]
    private GameObject Destroy;
    private int playersCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersCount++;

            if (playersCount == 1)
            {
                Destroy.SetActive(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersCount--;

            if (playersCount == 0)
            {
                Destroy.SetActive(true);
            }
        }
    }
}
