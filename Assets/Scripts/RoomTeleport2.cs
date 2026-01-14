using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomTeleport2 : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform destination;
    [SerializeField] private string playerTag = "Player";

    [Header("Confiner Settings")]
    [SerializeField] private Collider roomCameraBoundary;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("Camera Position Settings")]
    [Tooltip("Bifeaza daca vrei sa schimbi offset-ul camerei în aceasta camera nou?.")]
    [SerializeField] private bool changeCameraOffset = false;

    [Tooltip("Noua pozitie a camerei fata de jucator (Follow Offset).")]
    [SerializeField] private Vector3 newFollowOffset = new Vector3(0, 10, -10);

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

        CinemachineCore.Instance.OnTargetObjectWarped(player.transform, displacement);

        if (changeCameraOffset)
        {
            var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            if (transposer != null)
            {
                transposer.m_FollowOffset = newFollowOffset;
            }
            else
            {
                Debug.LogWarning("Virtual Camera nu are componenta 'CinemachineTransposer' setata la Body.");
            }
        }
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

        // 5. Reactivare CharacterController
        if (cc != null) cc.enabled = true;
    }
}
