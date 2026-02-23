using UnityEngine;

public class MultiBullet : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<MultiEnemy>().TakeDamage(damage);
            //instanciar vfx de sangre
        }
        Destroy(gameObject);
    }
}
