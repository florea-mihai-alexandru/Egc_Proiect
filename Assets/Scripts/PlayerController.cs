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
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.D)) x = 1;
        else if (Input.GetKey(KeyCode.A)) x = -1;

        if (Input.GetKey(KeyCode.W)) y = 1;
        else if (Input.GetKey(KeyCode.S)) y = -1;

        Vector3 moveDir = new Vector3(x, 0, y).normalized;
        //Debug.Log(x + y);
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
