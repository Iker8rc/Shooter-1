using UnityEngine;
using Photon.Pun;

public class MultiHeart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        MultiplayerController player = other.GetComponent<MultiplayerController>();
        if (player == null)
        {
            return;
        }
        if (player.photonView.IsMine == false)
        {
            return;
        }
        player.life +=1;
        FindObjectOfType<MultiLevelManager>().UpdateLife();
        PhotonNetwork.Destroy(gameObject);
    }
}
