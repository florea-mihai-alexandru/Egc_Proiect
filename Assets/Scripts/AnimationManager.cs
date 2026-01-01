using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Awake()  
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void PlayAnimation(Vector3 movementInput)
    {
        float x = movementInput.x;
        float y = movementInput.z;
        if (x == 0 && y == 0)
        {
            animator.Play("PlayerIdle");
        }
        else if (x != 0 && x < 0)
        {
            FlipX(false);
            animator.Play("PlayerWalkSide");
        }
        else if (x != 0 && x > 0)
        {
            FlipX(true);
            animator.Play("PlayerWalkSide");
        }
        else if (y > 0)
        {
            animator.Play("PlayerWalkForward");
        }
        else if (y < 0)
        {
            animator.Play("PlayerWalkBack");
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
}
