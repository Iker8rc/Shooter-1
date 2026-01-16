using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent <PlayerController>().TakeDamage(damage);
        }
        //Destroy(gameObject);
    }
}

