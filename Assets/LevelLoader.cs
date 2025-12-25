using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;        
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
            
    }
    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    // The Coroutine allows us to "pause" execution to let the animation play
    IEnumerator LoadLevel(string sceneName)
    {
        // 1. Play the Fade Out animation
        // Make sure your Animation Parameter is named exactly "Start" (Case Sensitive!)
        transition.SetTrigger("Start");

        // 2. Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        // 3. Load the Scene
        SceneManager.LoadScene(sceneName);
    }
}
