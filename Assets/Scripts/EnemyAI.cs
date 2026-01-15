using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public UnityEvent<Vector3> OnMoveEvent;
    public UnityEvent<bool> ToggleWalkEvent;
    public UnityEvent<bool> ToggleNavEvent;
    public UnityEvent<Vector3> OnAttack;

    private Vector3 randWalkDir = Vector3.zero;
    private Vector3 destination = Vector3.zero;

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

    float distanceToPlayer;
    Vector3 directionToPlayer;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    private float obstacleCheckRadius;

    [SerializeField]
    private float obstacleCheckDistance;

    [SerializeField]
    private LayerMask obstacleLayerMask;

    [SerializeField]
    private LayerMask wallLayerMask;

    [SerializeField]
    private float obstacleAvoidanceCooldown = 0.5f;

    private float obstacleAvoidanceTime = 0.5f;
    private Vector3 obstacleAvoidanceTargetDir;

    private RaycastHit[] obstacleCollisions;

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
        if (player.IsUnityNull())
        {
            GameObject objFound = GameObject.FindGameObjectWithTag("Player");
            player = objFound.transform;
            Debug.Log("Found Player automatically (" + gameObject.name + ")");
        }
    }

    private void Update()
    {
        switch (currentState)
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
        OnMoveEvent?.Invoke(randWalkDir);

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

        obstacleAvoidanceTime -= Time.deltaTime;
        if (obstacleAvoidanceTime <= 0)
        {
            directionToPlayer = player.position - transform.position;
            obstacleAvoidanceTime = obstacleAvoidanceCooldown;
        }

        directionToPlayer.y = 0;
        directionToPlayer.Normalize();
        Vector3 finalDir = HandleObstacles(directionToPlayer);

        OnMoveEvent?.Invoke(player.position);

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
                ToggleNavEvent?.Invoke(false);
                break;

            case AI_State.Patrolling:
                Vector3 rand = RandomNavSphere(transform.position, 10, -1);
                randWalkDir.x = rand.x;
                randWalkDir.z = rand.z;
                ToggleWalkEvent?.Invoke(true);
                ToggleNavEvent?.Invoke(true);

                patrolledTime = 0;
                break;

            case AI_State.Chasing:
                ToggleWalkEvent?.Invoke(false);
                ToggleNavEvent?.Invoke(true);
                break;

            case AI_State.Attacking:
                ToggleWalkEvent?.Invoke(false);
                ToggleNavEvent?.Invoke(false);
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

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistanceThreshold);
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 direction = getCurDir();

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position + direction*obstacleCheckDistance, obstacleCheckRadius);
    //}

    private Vector3 HandleObstacles(Vector3 direction)
    {
        direction.y = 0f;

        if ((obstacleCollisions = Physics.SphereCastAll(transform.position, obstacleCheckRadius, direction, obstacleCheckDistance, ~obstacleLayerMask)).Length>0)
        {
            foreach (var collision in obstacleCollisions)
            {
                if (collision.transform.gameObject == gameObject)
                {
                    continue;
                }

                obstacleAvoidanceTargetDir = collision.normal;
                Vector3 avoidDir = Vector3.ProjectOnPlane(direction, obstacleAvoidanceTargetDir);
                avoidDir.y = 0f;
                avoidDir.Normalize();

                if (avoidDir.sqrMagnitude > 0.001f)
                    return avoidDir.normalized;
                break;
            }
        }
        return direction;
    }


    private Vector3 getCurDir()
    {
        Vector3 direction = Vector3.zero;
        if (currentState == AI_State.Patrolling)
        {
            direction = randWalkDir;
        }
        else if (currentState == AI_State.Chasing)
        {
            direction = directionToPlayer;
        }
        return direction;
    }

    private void setCurDir(Vector3 direction)
    {
        if (currentState == AI_State.Patrolling)
        {
            randWalkDir = direction;
        }
        else if (currentState == AI_State.Chasing)
        {
            directionToPlayer = direction;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((wallLayerMask & (1 << collision.gameObject.layer)) != 0)
        {
            if (currentState == AI_State.Patrolling)
            {
                randWalkDir = -randWalkDir;
            }
        }
    }
    #endregion
}
