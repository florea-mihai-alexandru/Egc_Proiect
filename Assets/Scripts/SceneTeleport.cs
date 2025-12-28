using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{

    public string sceneToLoad;
    public string playerTag = "Player";
    public LevelLoader levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (levelLoader != null)
            {
                levelLoader.LoadNextLevel(sceneToLoad);
            }
        }
    }
}
