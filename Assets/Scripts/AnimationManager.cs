using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;

    float attackTime;
    float deathTime;

    float playingFor = 0;

    bool attacking = false;

    private void Awake()  
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateAnimClipTimes();
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
                    attackTime = clip.length;
                    break;
                case "Death":
                    deathTime = clip.length;
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
