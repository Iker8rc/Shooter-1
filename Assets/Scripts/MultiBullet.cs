using Photon.Pun;
using UnityEngine;

public class MultiBullet : MonoBehaviourPunCallbacks, IPunObservable
{
    public float damage;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            if (photonView.IsMine == true)
            {
                 collision.gameObject.GetComponent<MultiEnemy>().TakeDamage(damage, photonView.Owner);
                 //instanciar vfx de sangre
            }
            
        }
        Destroy(gameObject);
    }
}
