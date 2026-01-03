using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;

    public float attackTime;
    public float deathTime;

    public float playingFor = 0;

    private bool attacking = false;

    private Vector3 originalScale;

    private Vector3 idleScale;

    private AnimatorClipInfo[] curAnimatorClipInfos;
    private string curAnimName;

    private void Awake()  
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateAnimClipTimes();
        idleScale = new Vector3(0.2f, 0.2f, 1);
        originalScale = transform.localScale;
    }

    public void PlayAnimation(Vector3 movementInput)
    {
        float x = movementInput.x;
        float y = movementInput.z;

        if (attacking)
        {
            if (playingFor >= attackTime)
            {
                attacking = false;
                animator.SetBool("Attacking", attacking);
            }

            playingFor += Time.deltaTime;
        }

        animator.SetFloat("yVelocity", y);
        animator.SetFloat("xVelocity", x);

        animator.SetFloat("magnitude", movementInput.magnitude);

        //if (curAnimName.Equals("EnemyIdle"))
        //{
        //    gameObject.transform.localScale = idleScale;
        //}
        //else
        //{
        //    gameObject.transform.localScale = originalScale;
        //}

        if (x < 0)
        {
            FlipX(false);
        }
        else if (x > 0)
        {
            FlipX(true);
        }
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    Debug.Log("at");
                    attackTime = clip.length;
                    break;
                case "Death":
                    Debug.Log("de");
                    deathTime = clip.length;
                    break;
                case "Idle":
                    Debug.Log("id");
                    break;
            }
        }
    }

    public void FlipX(bool flip)
    {
        Vector3 scale = gameObject.transform.localScale;
        if (flip) 
        {
            scale.x = -math.abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            scale.x = math.abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void AttackAnim(Vector3 direction)
    {
        if (direction.x < 0)
        {
            FlipX(false);
        }
        else if (direction.x > 0)
        {
            FlipX(true);
        }
        attacking = true;
        animator.SetBool("Attacking", attacking);
        playingFor = 0;
    }
}
