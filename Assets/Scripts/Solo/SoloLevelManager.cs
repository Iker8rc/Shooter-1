using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoloLevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private GameObject[] lifeColor;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI killCount;
    private SoloLevelManager levelManager;
    private SoloPlayerController player;
    
    public GameObject heartPrefab;
    public Transform[] heartSpawnPoints;
    private List<GameObject> currentHearts = new List<GameObject>();

    [SerializeField] 
    private float heartSpawn = 90f; 
    [SerializeField] 
    private int hearts = 3;

    //Audio
    [SerializeField] 
    private AudioClip winMusic; 
    [SerializeField] 
    private AudioSource musicSource;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = FindObjectOfType<SoloLevelManager>();
        player = FindObjectOfType<SoloPlayerController>();
        StartCoroutine(HeartSpawnea());    
    }

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
    private IEnumerator HeartSpawnea()
    {
        while (true)
        {
            yield return new WaitForSeconds(heartSpawn);
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
                Destroy(heart);
            }          
        }
        currentHearts.Clear();
    }

    private void SpawnHearts()
    {
        for (int i = 0; i < hearts; i++)
        {
            Transform randomPoint = heartSpawnPoints[Random.Range(0, heartSpawnPoints.Length)];

            GameObject newHeart = Instantiate(heartPrefab, randomPoint.position, randomPoint.rotation); 
            currentHearts.Add(newHeart);
        }
    }

    public void UpdateKills()
    {
        killCount.text = "x" + player.totalKills.ToString();
    }

    public void Win()
    {
        musicSource.Stop();
        musicSource.clip = winMusic;
        musicSource.Play();
        winPanel.SetActive(true);
    }
}
