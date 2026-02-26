using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject panelGameOver;
    [SerializeField] 
    private GameObject panelPause;
    private MultiplayerController myPlayer;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myPlayer = FindAnyObjectByType<MultiplayerController>();
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
            myPlayer = FindAnyObjectByType<MultiplayerController>();
        }

        if (panelPause.activeInHierarchy == false)
        {
            panelPause.SetActive(true);

            if (myPlayer.photonView.IsMine)
            {
                myPlayer.isPaused = true;   
            }
        }
        else
        {
            panelPause.SetActive(false);

            if (myPlayer.photonView.IsMine)
            {
                myPlayer.isPaused = false; 
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
