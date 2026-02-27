using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = FindObjectOfType<SoloLevelManager>();
        player = FindObjectOfType<SoloPlayerController>();
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

    public void UpdateKills()
    {
        killCount.text = "x" + player.totalKills.ToString();
    }

    public void Win()
    {
        winPanel.SetActive(true);
    }
}
