using UnityEngine;
using UnityEngine.SceneManagement;

public class SoloMainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField]
    private GameObject panelPause;
    private SoloPlayerController myPlayer;

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
        //AudioManager.Instance.SetMusicVolume(0.6f);
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        //AudioManager.Instance.FadeOutMusic(1.5f);
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
            panelPause.SetActive(true);
        }
        else
        {
            panelPause.SetActive(false);
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
