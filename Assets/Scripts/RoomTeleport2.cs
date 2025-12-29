using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomTeleport2 : MonoBehaviour
{
    [Header("Where to go")]
    [Tooltip("The empty object where the player spawns. ROTATE this object to set player facing direction.")]
    [SerializeField] private Transform destination;

    [Header("Camera Settings")]
    [Tooltip("The Collider defining the NEW room's boundaries.")]
    [SerializeField] private Collider newRoomBoundary;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PerformTransition(other.gameObject);
        }
    }

    private void PerformTransition(GameObject player)
    {
        // 1. Disable CharacterController to prevent physics conflicts
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 2. Calculate displacement for the Camera Warp
        Vector3 displacement = destination.position - player.transform.position;

        // 3. TELEPORT POSITION
        player.transform.position = destination.position;

        // 4. TELEPORT ROTATION (Crucial for your case)
        // The player will instantly snap to face the same direction as the "Destination" object.
        player.transform.rotation = destination.rotation;

        // 5. CAMERA: Instant Warp
        // This stops the camera from panning smoothly across the void. It cuts instantly.
        CinemachineCore.Instance.OnTargetObjectWarped(player.transform, displacement);

        // 6. CAMERA: Switch Room Boundaries
        // Instead of resizing the collider, we just swap the reference. Much safer.
        if (virtualCamera != null && newRoomBoundary != null)
        {
            CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            if (confiner != null)
            {
                confiner.m_BoundingVolume = newRoomBoundary;
                confiner.InvalidatePathCache();
            }
        }

        // 7. Re-enable CharacterController
        if (cc != null) cc.enabled = true;
    }
}
