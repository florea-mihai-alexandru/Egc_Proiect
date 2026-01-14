using UnityEngine;

public class RoomObjective : MonoBehaviour
{
    [Header("Obiective")]
    [Tooltip("Pune aici obiectul părinte care conține toți inamicii sau obiectele de distrus.")]
    public Transform targetsParent;

    [Tooltip("Nume unic pentru această cameră (ex: Room1, ForestLevel, etc.)")]
    public string roomIdentifier;

    private bool isDone = false;

    void Update()
    {
        if (isDone || targetsParent == null) return;

        // Verificăm dacă obiectul părinte mai are copii
        if (targetsParent.childCount == 0)
        {
            CompleteRoom();
        }
    }

    void CompleteRoom()
    {
        isDone = true;

        if (GameProgressionManager.Instance != null)
        {
            GameProgressionManager.Instance.MarkRoomAsComplete(roomIdentifier);
        }
        else
        {
            Debug.LogWarning("Nu am găsit GameProgressionManager! Asigură-te că ai pornit jocul din scena corectă.");
        }
    }
}