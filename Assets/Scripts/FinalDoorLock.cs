using UnityEngine;

public class FinalDoorLock : MonoBehaviour
{
    void Start()
    {
        CheckLock();
    }

    private void OnEnable()
    {
        CheckLock();
    }

    void CheckLock()
    {
        if (GameProgressionManager.Instance == null) return;

        if (GameProgressionManager.Instance.CanUnlockFinalRoom())
        {
            // Dacă ai terminat cele 4 camere, dezactivăm bariera/ușa
            gameObject.SetActive(false);
        }
        else
        {
            // Altfel, rămâne activă și te blochează
            gameObject.SetActive(true);
        }
    }
}