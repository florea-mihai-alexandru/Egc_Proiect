using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTeleport : MonoBehaviour
{
    [Tooltip("Scrie aici numele scenei Main Menu")]
    public string sceneToLoad = "MainMenu";

    public LevelLoader levelLoader;
    public PlayerStats playerstats; // Presupunem că ai deja scriptul PlayerStats

    private bool hasTriggered = false;

    void Update()
    {
        // Folosim <= 0 pentru siguranță
        if (playerstats.Health <= 0 && !hasTriggered)
        {
            DieAndReset();
        }
    }

    void DieAndReset()
    {
        hasTriggered = true;

        // 1. Resetăm progresul global
        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.ResetProgress();
        }

        // 2. Încărcăm scena
        if (levelLoader != null)
        {
            levelLoader.LoadNextLevel(sceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}