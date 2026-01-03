using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public UnityEvent<Vector3> OnMoveEvent;
    public UnityEvent<bool> ToggleWalkEvent;
    public UnityEvent<Vector3> OnAttack;

    private Vector3 randWalkDir = Vector3.zero;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float chaseDistanceThreshold = 3, attackDistanceThreshold = 0.8f;

    [SerializeField]
    private float attackDelay = 1.2f;
    private float passedAttackTime = 1.2f;

    [SerializeField]
    private float patrolTime = 3;  //Time until changes state to idle
    private float patrolledTime = 0;

    [SerializeField]
    private float idleTime = 2;  //Time until changes state to patrolling 
    private float idledForTime = 0;

    [SerializeField]
    private float wallRange = 1.5f;

    float distanceToPlayer;
    Vector3 directionToPlayer;

    #region State Variables
    private enum AI_State
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking
    }

    AI_State currentState;
    #endregion

    #region Base Start and Update Method
    private void Start()
    {
        currentState = AI_State.Patrolling;
        InitState();
    }

    private void Update()
    {
        switch(currentState)
        {
            case AI_State.Idle:
                IdleStateUpdate();
                //Debug.Log("Idle");
                break;

            case AI_State.Patrolling:
                PatrollStateUpdate();
                break;

            case AI_State.Chasing:
                ChasingStateUpdate();
                //Debug.Log("Chasing");
                break;

            case AI_State.Attacking:
                AttackingStateUpdate();
                //Debug.Log("Attack");
                break;
        }
    }
    #endregion

    #region State UPDATES
    private void IdleStateUpdate()
    {
        OnMoveEvent?.Invoke(Vector3.zero);

        idledForTime += Time.deltaTime;
        if (idledForTime > idleTime)
        {
            SwitchState(AI_State.Patrolling);
        }

        if (player == null)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackDistanceThreshold)
        {
            SwitchState(AI_State.Attacking);
        }
        else if (distanceToPlayer < chaseDistanceThreshold)
        {
            SwitchState(AI_State.Chasing);
        }
    }

    private void PatrollStateUpdate()
    {
        OnMoveEvent?.Invoke(randWalkDir.normalized);

        patrolledTime += Time.deltaTime;
        if (patrolledTime > patrolTime)
        {
            SwitchState(AI_State.Idle);
        }

        if (player == null)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attackDistanceThreshold)
        {
            SwitchState(AI_State.Attacking);
        }
        else if (distanceToPlayer < chaseDistanceThreshold)
        {
            SwitchState(AI_State.Chasing);
        }
    }

    private void ChasingStateUpdate()
    {
        if (player.IsDestroyed())
        {
            SwitchState(AI_State.Idle);
            return;
        }
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        directionToPlayer = player.position - transform.position;

        OnMoveEvent?.Invoke(directionToPlayer.normalized);

        if (passedAttackTime < attackDelay)
        {
            passedAttackTime += Time.deltaTime;
        }

        if (distanceToPlayer < attackDistanceThreshold)
        {
            SwitchState(AI_State.Attacking);
        }
        else if (distanceToPlayer > chaseDistanceThreshold)
        {
            SwitchState(AI_State.Patrolling);
        }
    }

    private void AttackingStateUpdate()
    {
        if (player.IsDestroyed())
        {
            SwitchState(AI_State.Idle);
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        directionToPlayer = player.position - transform.position;

        OnMoveEvent?.Invoke(Vector3.zero);
        if (passedAttackTime >= attackDelay)
        {
            OnAttack?.Invoke(directionToPlayer.normalized);
            passedAttackTime = 0;
        }

        if (passedAttackTime < attackDelay)
        {
            passedAttackTime += Time.deltaTime;
        }

        if (distanceToPlayer > attackDistanceThreshold && passedAttackTime > attackDelay)
        {
            SwitchState(AI_State.Chasing);
        }
    }
    #endregion

    #region State Methods
    private void SwitchState(AI_State newState)
    {
        currentState = newState;
        InitState();
    }    

    private void InitState()
    {
        switch(currentState)
        {
            case AI_State.Idle:
                idledForTime = 0;
                ToggleWalkEvent?.Invoke(false);
                break;

            case AI_State.Patrolling:
                Vector2 rand = RandomUnitVector();
                randWalkDir.x = rand.x;
                randWalkDir.z = rand.y;
                ToggleWalkEvent?.Invoke(true);

                patrolledTime = 0;
                break;

            case AI_State.Chasing:
                ToggleWalkEvent?.Invoke(false);
                break;

            case AI_State.Attacking:
                ToggleWalkEvent?.Invoke(false);
                break;
        }
    }
    #endregion

    #region Other Methods
    public Vector2 RandomUnitVector()
    {
        float random = Random.Range(0f, 260f);
        return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    }

    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, wallRange);
    }

    private void CheckIfWall()
    {

    }
    #endregion
}
