using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 
public class RoomTeleport : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private Collider roomCameraBoundary;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            TeleportAndSwitchCamera(other.gameObject);
        }
    }

    private void TeleportAndSwitchCamera(GameObject player)
    {
        if (destination == null) return;

        // 1. Move the Player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 displacement = destination.position - player.transform.position;
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation;

        // 2. Snap the Camera (Prevents the Y-axis sliding)
        if (virtualCamera != null)
        {
            // Tell Cinemachine the target warped
            CinemachineCore.Instance.OnTargetObjectWarped(player.transform, displacement);

            // 3. Update the Camera Confiner to the new room's box
            CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            if (confiner != null && roomCameraBoundary != null)
            {
                confiner.m_BoundingVolume = roomCameraBoundary;

                // This clears the old boundary data so the camera snaps to the new box immediately
                confiner.InvalidatePathCache();
            }
        }

        if (cc != null) cc.enabled = true;
    }
}
