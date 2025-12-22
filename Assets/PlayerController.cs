using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public Rigidbody rb;
    public SpriteRenderer sr;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();        
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, y);

        rb.velocity = moveDir * speed;

        if (x == 0 && y == 0)
        {
            animator.Play("PlayerIdle");
        }
        else if (x != 0 && x < 0)
        {
            sr.flipX = false;
            animator.Play("PlayerWalkSide");
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = true;
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
}
