using UnityEngine;
using UnityEngine.SceneManagement; // 1. THIS LINE IS REQUIRED

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        // 2. CHECK THE SCENE NAME
        // Replace "MainMenu" with the EXACT name of your menu scene
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // If we are in the menu, destroy this persistent gameplay music
            // so the menu's own music can take over.
            Destroy(gameObject);
        }
    }
}