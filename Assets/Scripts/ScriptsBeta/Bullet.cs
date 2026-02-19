using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

public class Bullet : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private GameObject bulletHolePrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            //instanciar vfx de sangre
        }
        else if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            //instanciar vfx de sangre
        }
        else
        {
            Quaternion rotacion = Quaternion.FromToRotation(Vector3.up, collision.GetContact(0).normal);
            GameObject bulletHoleClone = Instantiate(bulletHolePrefab, collision.GetContact(0).point, rotacion, collision.transform);
            bulletHoleClone.transform.localPosition += new Vector3(0, 0, -0.02f);
        }
        Destroy(gameObject);
    }

    //ESTO ES PARA VIDEO

    /* VideoPlayer videoPlayer;
     private void OnTriggerEnter(Collider other)
     {
         videoPlayer.Play();
         videoPlayer.Stop();
         videoPlayer.Pause();
         videoPlayer.clip
     }*/
    PlayableDirector videoRT;

    /*private void OnTriggerEnter(Collider other)
    {
        videoRT.Play();
        videoRT.Pause();
        videoRT.Stop();
        videoRT.state == PlayState.Playing;
    }*/
}

