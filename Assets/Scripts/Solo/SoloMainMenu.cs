using UnityEngine;
using UnityEngine.SceneManagement;

public class SoloMainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private GameObject panelPause;
    private SoloPlayerController myPlayer;
    [SerializeField] 
    private AudioSource musicSource;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myPlayer = FindAnyObjectByType<SoloPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        musicSource.volume = 0.1f;
        panelGameOver.SetActive(true);
    }
    public void Pause()
    {
        if (myPlayer == null)
        {
            myPlayer = FindAnyObjectByType<SoloPlayerController>();
        }

        if (panelPause.activeInHierarchy == false)
        {
            musicSource.volume = 0.1f;
            panelPause.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            musicSource.volume = 0.6f;
            panelPause.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void GameOverPanel()
    {
        panelGameOver.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
