using UnityEngine;

public class SoloBullet : MonoBehaviour
{
    public float damage;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<SoloEnemyController>().TakeDamage(damage);
        }
    }
}
