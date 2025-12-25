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
        // DEBUG 1: Did anything touch me?
        //Debug.Log("COLLISION DETECTED with: " + other.gameObject.name);

        if (other.CompareTag(playerTag))
        {
            // DEBUG 2: Was it the player?
            //Debug.Log("It is the Player! Calling LevelLoader...");

            if (levelLoader != null)
            {
                levelLoader.LoadNextLevel(sceneToLoad);
            }
            else
            {
                //Debug.LogError("ERROR: LevelLoader slot is EMPTY on the Door!");
            }
        }
        else
        {
            // DEBUG 3: It wasn't the player
            //Debug.Log("Object is not tagged 'Player'. It is tagged: " + other.tag);
        }
    }
}
