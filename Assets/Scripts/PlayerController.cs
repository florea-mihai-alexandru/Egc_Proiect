using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool dead = false;

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
        if (Time.timeScale == 0f) return;

        move(MoveDir, speed);
        animationManager.PlayAnimation(moveDir);

        if(stats.Health <= 0)
        {
            if (!dead)
            {
                StartCoroutine(ExecuteDeath());
            }
        }
    }

    public void PerformAttack(Vector3 direction)
    {
        if (!dead)
        {
            //combatManager.Attack(direction);
            //animationManager.AttackAnim(direction);
            StartCoroutine(AttackAction(direction));
        }
    }

    public IEnumerator AttackAction(Vector3 direction)
    {
        combatManager.Attack(direction);
        yield return combatManager.currentWeapon.attackSpeed;
        animationManager.AttackAnim(direction);
    }

    public IEnumerator ExecuteDeath()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        dead = true;
        yield return animationManager.DeathAnim();
        Destroy(gameObject);
    }

    public void move(Vector3 direction, float speed)
    {
        if (dead)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            if (isWalking)
            {
                rb.velocity = direction * speed * walkSlowdown;
                animationManager.PlayAnimation(direction);
            }
            else
            {
                rb.velocity = direction * speed;
                animationManager.PlayAnimation(direction);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        stats.TakeDamage(dmg);
    }
}
