using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void Home()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OpenOptions()
    {
        Time.timeScale = 1f; 

        PlayerPrefs.SetInt("OpenOptionsAtStart", 1);

        SceneManager.LoadScene("MainMenu"); 
    }
}