using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using ExitGames.Client.Photon;


public class MultiLevelManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public Transform[] spawnPoints;
    [SerializeField]
    private GameObject[] lifeColor;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField] 
    private TMPro.TextMeshProUGUI killCount;
    private MultiLevelManager levelManager;

    public GameObject heartPrefab;
    public Transform[] heartSpawnPoints;
    private List<GameObject> currentHearts = new List<GameObject>();

    [SerializeField] 
    private float heartSpawnInterval = 90f; 
    [SerializeField] 
    private int hearts = 3;

    public MultiplayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.Instantiate("Player", spawnPoints[0].position, spawnPoints[0].rotation);
        StartCoroutine(HeartSpawnea());    
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private IEnumerator HeartSpawnea()
    {
        while (true)
        {
            yield return new WaitForSeconds(heartSpawnInterval);
            DestroyHearts();
            SpawnHearts();
        }
    }
    private void DestroyHearts()
    {
        foreach (GameObject heart in currentHearts)
        {
            if (heart != null)
            {
                PhotonNetwork.Destroy(heart);
            }          
        }

        currentHearts.Clear();
    }

    private void SpawnHearts()
    {
        for (int i = 0; i < hearts; i++)
        {
            Transform randomPoint = heartSpawnPoints[Random.Range(0, heartSpawnPoints.Length)];

            GameObject newHeart = PhotonNetwork.Instantiate("Heart", randomPoint.position, randomPoint.rotation); 
            currentHearts.Add(newHeart);
        }
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
    public void Win()
    {
        PhotonView photonV = GetComponent<PhotonView>();
        photonV.RPC("RPC_WinPanel", RpcTarget.All);    
    }

    [PunRPC]
    void RPC_WinPanel()
    {
        winPanel.SetActive(true);
    }
}
