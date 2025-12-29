using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent<Vector3> OnAttack;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 3, attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1.2f;
    private float passedTime = 1.2f;

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < chaseDistanceThreshold)
        {
            Vector3 direction = player.position - transform.position;
            if (distance <= attackDistanceThreshold)
            {
                OnMovementInput?.Invoke(Vector3.zero);
                if(passedTime >= attackDelay)
                {
                    OnAttack?.Invoke(direction.normalized);
                }
            }
            else
            {
                OnMovementInput?.Invoke(direction.normalized);
            }
        }

        if (passedTime <  attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }
}
