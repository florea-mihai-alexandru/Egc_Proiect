using UnityEngine;
using System.Collections.Generic;

public class WaveRoomManager : MonoBehaviour
{
    [Header("Room Settings")]
    [Tooltip("The tag required to trigger the room (usually 'Player').")]
    public string playerTag = "Player";

    [Tooltip("Drag the object that BLOCKS the door here (e.g., a wall or door with a BoxCollider). It will be enabled when the room starts.")]
    public GameObject doorBlocker;

    [Header("Wave Configuration")]
    [Tooltip("Drag your 12 enemies here in the order you want them to spawn. (0-2 = Wave 1, 3-5 = Wave 2, etc.)")]
    public List<GameObject> allEnemies;

    [Tooltip("How many enemies spawn per wave?")]
    public int enemiesPerWave = 3;

    // State tracking
    private List<GameObject> currentActiveEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;
    private bool roomStarted = false;
    private bool roomFinished = false;

    private void Start()
    {
        // 1. Hide all enemies at the start
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null) enemy.SetActive(false);
        }

        // 2. Ensure the door blocker is hidden (open) at start
        if (doorBlocker != null)
        {
            doorBlocker.SetActive(false);
        }
    }

    private void Update()
    {
        // Only run logic if the room has started and hasn't finished
        if (!roomStarted || roomFinished) return;

        // 3. Check if current wave is dead
        // We look through the active list. If an enemy is destroyed, it becomes 'null'.
        // RemoveAll(x => x == null) removes the dead enemies from our tracking list.
        currentActiveEnemies.RemoveAll(enemy => enemy == null);

        // If the list is empty, the wave is cleared
        if (currentActiveEnemies.Count == 0)
        {
            SpawnNextWave();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Start the room when player enters, but only once
        if (other.CompareTag(playerTag) && !roomStarted)
        {
            StartRoom();
        }
    }

    private void StartRoom()
    {
        roomStarted = true;

        // Lock the door behind the player
        if (doorBlocker != null)
        {
            doorBlocker.SetActive(true);
            Debug.Log("Door Locked!");
        }

        // Spawn the first wave
        SpawnNextWave();
    }

    private void SpawnNextWave()
    {
        // Calculate which enemies belong to the next wave
        int startIndex = currentWaveIndex * enemiesPerWave;
        int endIndex = startIndex + enemiesPerWave;

        // Check if we have run out of enemies (Room Cleared)
        if (startIndex >= allEnemies.Count)
        {
            FinishRoom();
            return;
        }

        Debug.Log($"Spawning Wave {currentWaveIndex + 1}...");

        // Loop through the specific slice of 3 enemies for this wave
        for (int i = startIndex; i < endIndex; i++)
        {
            // Safety check: make sure the index exists in the list
            if (i < allEnemies.Count && allEnemies[i] != null)
            {
                allEnemies[i].SetActive(true);
                currentActiveEnemies.Add(allEnemies[i]);
            }
        }

        currentWaveIndex++;
    }

    private void FinishRoom()
    {
        roomFinished = true;
        Debug.Log("Room Cleared! Opening doors.");

        // Unlock the door
        if (doorBlocker != null)
        {
            doorBlocker.SetActive(false);
        }

        // Optional: Destroy the trigger so it doesn't happen again
        // Destroy(gameObject);
    }

    // Visualize the trigger box in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}