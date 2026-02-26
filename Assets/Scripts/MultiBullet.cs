using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class MultiBullet : MonoBehaviourPunCallbacks, IPunObservable
{
    public float damage;
    public Player owner;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (photonView.IsMine == true)
            {
                 collision.gameObject.GetComponent<MultiEnemy>().TakeDamage(damage, owner);
            }            
        }
        PhotonNetwork.Destroy(gameObject);
    }
}