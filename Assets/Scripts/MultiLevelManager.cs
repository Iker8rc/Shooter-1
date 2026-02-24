using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MultiLevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private TextMeshProUGUI bulletMagazine, totalBullets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate("MultiplayerPlayer", spawnPoints[0].position, spawnPoints[0].rotation);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void UpdateBullets()
    {
        bulletMagazine.text = GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].MagazineBullets;
        totalBullets.text = GameManager.instance.GetGameData.Weapon[GameManager.instance.GetGameData.WeaponIndex].TotalBullets;
    }
    public void UpdateLife()
    {
        float percentage = 1 - (GameManager.instance.GetGameData.CurrentLife/ GameManager.instance.GetGameData.MaxLife);
    }
    /*public void UpdateKills()
    {
        killCount.text = "x" + MultiplayerManager.instance.totalKills.ToString();
    }*/
}
