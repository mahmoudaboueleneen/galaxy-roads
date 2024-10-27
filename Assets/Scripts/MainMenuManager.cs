using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject howToPlayPanel;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMenuTrack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void ShowOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void HideOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }

    public void ShowCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }

    public void ShowHowToPlayPanel()
    {
        howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlayPanel()
    {
        howToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
