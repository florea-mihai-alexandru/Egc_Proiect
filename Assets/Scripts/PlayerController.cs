using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Rigidbody rb;

    private Vector3 moveDir;
    private Vector3 attackDir;

    private CombatManager combatManager;
    private AnimationManager animationManager;

    private PlayerStats stats;

    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector3 AttackDir { get => attackDir; set => attackDir = value; }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();        
        combatManager = gameObject.GetComponentInChildren<CombatManager>();
        animationManager = gameObject.GetComponentInChildren<AnimationManager>();
        stats = gameObject.GetComponentInChildren<PlayerStats>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        move(MoveDir);
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

    public void move(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }

    public void TakeDamage(float dmg)
    {
        stats.TakeDamage(dmg);
    }
}
