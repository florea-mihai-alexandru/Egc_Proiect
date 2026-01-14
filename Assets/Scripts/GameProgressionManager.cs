using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance;

    [Header("Configurare")]
    [Tooltip("Numărul de camere necesare pentru a debloca ultima cameră")]
    public int requiredRoomsCount = 4;

    // Numele scenei de meniu (pentru auto-resetare dacă pornești jocul de acolo)
    public string mainMenuSceneName = "MainMenu";

    // Set pentru a ține minte camerele unice completate
    private HashSet<string> completedRooms = new HashSet<string>();

    void Awake()
    {
        // Singleton Pattern - asigură că există un singur manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Dacă jocul pornește direct din Meniu, ne asigurăm că progresul e zero
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            ResetProgress();
        }
    }

    public void MarkRoomAsComplete(string roomName)
    {
        if (!completedRooms.Contains(roomName))
        {
            completedRooms.Add(roomName);
            Debug.Log($"Camera '{roomName}' completată! Total: {completedRooms.Count}/{requiredRoomsCount}");
        }
    }

    public bool CanUnlockFinalRoom()
    {
        return completedRooms.Count >= requiredRoomsCount;
    }

    public void ResetProgress()
    {
        completedRooms.Clear();
        Debug.Log("Progres resetat (Game Over sau New Game).");
    }
}