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
    private GameObject[] lifeColor;
    [SerializeField] 
    private TMPro.TextMeshProUGUI killCount;

    private MultiplayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate("MultiplayerPlayer", spawnPoints[0].position, spawnPoints[0].rotation);
        player = FindObjectOfType<MultiplayerController>();    }

    // Update is called once per frame
    void Update()
    {
      
    }
    public void UpdateLife()
    {
        if (player == null) 
        {
            return;
        }

        int currentLife = (int)player.life;
        for (int i = 0; i < lifeColor.Length; i++)
        {
            lifeColor[i].SetActive(i < currentLife);
        }
    }
    
    public void UpdateKills()
    {
        killCount.text = "x" + player.totalKills.ToString();
    }
}
