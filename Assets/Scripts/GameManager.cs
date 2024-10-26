using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        ResumeGame();
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
