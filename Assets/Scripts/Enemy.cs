using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float deathTime;
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateAnimClipTimes();
    }

    void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    public void UpdateAnimClipTimes() // Daca mai avem nevoie de lungimi de clipuri pentru anumite delay uri actualizati aceasta functie!
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals("EnemyDeath"))
            {
                deathTime = clip.length;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("A LUAT DAMAGE !!!");
    }

    public void Death()
    {
        animator.Play("Death");
        StartCoroutine(WaitForDeathAnimToFinish());
    }

    IEnumerator WaitForDeathAnimToFinish()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(gameObject);
    }
}