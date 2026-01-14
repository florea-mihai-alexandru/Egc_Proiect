using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
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

    //private Vector3 originalScale;

    //private Vector3 idleScale;

    //private AnimatorClipInfo[] curAnimatorClipInfos;
    //private string curAnimName;

    [SerializeField]
    private Animator pixAnimator;

    [SerializeField]
    private Animator keyboardAnimator;

    [SerializeField]
    private Animator mouseAnimator;

    private int curWeaponIndex;

    public GameObject keyExplosionObject;
    public GameObject inkExplosionObject;
    private GameObject ExplosionInstance;

    public Transform attackPos;

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
        //idleScale = new Vector3(0.2f, 0.2f, 1);
        //originalScale = transform.localScale;
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
                if(gameObject.tag != "Player")  
                {
                    animator.SetBool("Attacking", attacking);
                }

                if (gameObject.tag == "Player")
                {
                    Animator curAnimator = getCurWeaponAnimator();
                    curAnimator.SetBool("Attack", attacking);
 
                    if (curAnimator == keyboardAnimator)
                    {
                        AddExplosion(keyExplosionObject);
                    }
                    else if (curAnimator == pixAnimator)
                    {
                        AddInkExplosion(inkExplosionObject);
                    }
                }
            
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

    public void UpdateWeaponAnimations()
    {
        if (gameObject.tag == "Player")
        {
            AnimationClip[]  clips = getCurWeaponAnimator().runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Contains("Attack"))
                {
                    attackTime = clip.length;
                    break;
                }
            }
        }
    }

    public void AttackAnim(Vector3 direction)
    {
        UpdateWeaponAnimations();
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
        if(gameObject.tag != "Player")  
        {
            animator.SetBool("Attacking", attacking);
        }

        if(gameObject.tag == "Player")
        {
            Animator curAnimator = getCurWeaponAnimator();
            curAnimator.SetBool("Attack", attacking);
        }
        playingFor = 0;
    }

    public WaitForSeconds DeathAnim()
    {
        animator.SetBool("Death", true);
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

    private void AddExplosion(GameObject Explosion)
    {
        ExplosionInstance = Instantiate(Explosion);
        //ExplosionInstance = Instantiate(Explosion, attackPos.localPosition, Quaternion.identity);
        ExplosionInstance.transform.SetPositionAndRotation(attackPos.position + new Vector3(-2.3f , 1f, 0.3f), attackPos.rotation);
        //ExplosionInstance.transform.SetPositionAndRotation(attackPos.position, attackPos.rotation);
        Vector3 direction = attackPos.position - gameObject.transform.position;
        ExplosionInstance.transform.position += Vector3.right * direction.x * 1.2f;
        Debug.Log(gameObject.transform.position + " " + ExplosionInstance.transform.position + "AICIIIII");
    }

    private void AddInkExplosion(GameObject Explosion)
    {
        ExplosionInstance = Instantiate(Explosion);
        //ExplosionInstance = Instantiate(Explosion, attackPos.localPosition, Quaternion.identity);
        ExplosionInstance.transform.SetPositionAndRotation(attackPos.position + new Vector3(5.5f , -5f, 0.3f), attackPos.rotation);
        //ExplosionInstance.transform.SetPositionAndRotation(attackPos.position, attackPos.rotation);
        Vector3 direction = attackPos.position - gameObject.transform.position;
        ExplosionInstance.transform.position += Vector3.right * direction.x;
        Debug.Log(gameObject.transform.position + " " + ExplosionInstance.transform.position + "AICIIIII");
    }
}
