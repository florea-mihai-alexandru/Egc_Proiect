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
        if (destination == null || virtualCamera == null) return;

        // 1. Move the Player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 displacement = destination.position - player.transform.position;
        player.transform.position = destination.position;
        player.transform.rotation = destination.rotation; // Optional if you want to rotate player

        // 2. Snap the Camera (Prevents the Y-axis sliding)
        CinemachineCore.Instance.OnTargetObjectWarped(player.transform, displacement);

        // 3. UPDATE THE SHARED CONFINER BOX
        CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();

        // Check if we have a valid confiner and a "Blueprint" collider for the new room
        if (confiner != null && confiner.m_BoundingVolume != null && roomCameraBoundary != null)
        {
            // Get the "Master" collider (the one the camera is actually using)
            BoxCollider masterCollider = confiner.m_BoundingVolume as BoxCollider;
            BoxCollider targetBlueprint = roomCameraBoundary as BoxCollider;

            if (masterCollider != null && targetBlueprint != null)
            {
                // COPY DATA: Move the master box to the new room's position
                masterCollider.transform.position = targetBlueprint.transform.position;
                masterCollider.transform.rotation = targetBlueprint.transform.rotation;

                // COPY SIZE: Match the size (in case you have bigger Boss rooms!)
                masterCollider.size = targetBlueprint.size;
                masterCollider.center = targetBlueprint.center;

                // IMPORTANT: Tell Cinemachine the shape has changed
                confiner.InvalidatePathCache();
            }
        }

        if (cc != null) cc.enabled = true;
    }
}
