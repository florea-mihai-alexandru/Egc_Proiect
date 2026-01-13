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

    [SerializeField]
    private Animator pixAnimator;

    [SerializeField]
    private Animator keyboardAnimator;

    [SerializeField]
    private Animator mouseAnimator;

    private int curWeaponIndex;

    public int CurWeaponIndex { get => curWeaponIndex; set => curWeaponIndex = value; }

    private void Awake()  
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("ATENTIE, ANIMATOR NULL");
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
                //animator.SetBool("Attacking", attacking);

                Animator curAnimator = getCurWeaponAnimator();
                curAnimator.SetBool("Attack", attacking);
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

        //if (x <= 0)
        //{
        //    FlipX(false);
        //}
        //else if (x > 0)
        //{
        //    FlipX(true);
        //}
    }

    public void UpdateAnimClipTimes()
    {
        Debug.Log("UPDATING " + gameObject.name);
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
                    deathTime = clip.length;
                    Debug.Log("de" + deathTime);
                    break;
                case "Idle":
                    Debug.Log("id");
                    break;
            }
        }
    }

    public void AttackAnim(Vector3 direction)
    {
        //Debug.Log("ataca" + gameObject.name);
        //if (direction.x <= 0)
        //{
        //    FlipX(false);
        //}
        //else if (direction.x > 0)
        //{
        //    FlipX(true);
        //}

        attacking = true;
        //animator.SetBool("Attacking", attacking);

        Animator curAnimator = getCurWeaponAnimator();
        curAnimator.SetBool("Attack", attacking);

        playingFor = 0;
    }

    public WaitForSeconds DeathAnim()
    {
        animator.SetBool("Death", true);
        Debug.Log(deathTime);
        return new WaitForSeconds(deathTime);
    }

    public Animator getCurWeaponAnimator()
    {
        switch (curWeaponIndex)
        {
            case 0:
                return pixAnimator;
            case 1:
                return keyboardAnimator;
            case 2:
                return mouseAnimator;
        }
        return null;
    }
}
