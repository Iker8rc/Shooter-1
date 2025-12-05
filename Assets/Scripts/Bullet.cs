using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

        }
    }
}

