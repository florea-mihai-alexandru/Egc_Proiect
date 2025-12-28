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

    void Start()
    {
        
    }

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

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Vector3 displacement = destination.position - player.transform.position;
        player.transform.position = destination.position;
        //player.transform.rotation = destination.rotation; 

        CinemachineCore.Instance.OnTargetObjectWarped(player.transform, displacement);

        CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();

        
        if (confiner != null && confiner.m_BoundingVolume != null && roomCameraBoundary != null)
        {
            
            BoxCollider masterCollider = confiner.m_BoundingVolume as BoxCollider;
            BoxCollider targetBlueprint = roomCameraBoundary as BoxCollider;

            if (masterCollider != null && targetBlueprint != null)
            {
                masterCollider.transform.position = targetBlueprint.transform.position;
                masterCollider.transform.rotation = targetBlueprint.transform.rotation;

                masterCollider.size = targetBlueprint.size;
                masterCollider.center = targetBlueprint.center;

                confiner.InvalidatePathCache();
            }
        }

        if (cc != null) cc.enabled = true;
    }
}
