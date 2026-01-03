using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float speed;

    public float walkSlowdown = 0.25f;

    public Rigidbody rb;

    private Vector3 moveDir = Vector3.zero;
    private Vector3 attackDir;

    private CombatManager combatManager;
    private AnimationManager animationManager;

    private PlayerStats stats;

    private bool isWalking = false;

    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector3 AttackDir { get => attackDir; set => attackDir = value; }
    public bool IsWalking { get => isWalking; set => isWalking = value; }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();        
        combatManager = gameObject.GetComponentInChildren<CombatManager>();
        animationManager = gameObject.GetComponentInChildren<AnimationManager>();
        stats = gameObject.GetComponentInChildren<PlayerStats>();
    }

    void Update()
    {
        move(MoveDir, speed);
        animationManager.PlayAnimation(moveDir);

        if(stats.Health <= 0)
        {
            ExecuteDeath();
        }
    }

    public void PerformAttack(Vector3 direction)
    {
        combatManager.Attack(direction);
    }

    public void ExecuteDeath()
    {
        Destroy(gameObject);
    }

    public void move(Vector3 direction, float speed)
    {
        if (isWalking)
        {
            rb.velocity = direction * speed * walkSlowdown;
        }
        else
        {
            rb.velocity = direction * speed;
        }
    }

    public void TakeDamage(float dmg)
    {
        stats.TakeDamage(dmg);
    }
}
