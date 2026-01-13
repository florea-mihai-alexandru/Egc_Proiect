using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : PlayerController
{
    NavMeshAgent navigationAgent;

    private Vector3 pointToFollow;
    private bool isNavigating;

    public bool IsNavigating { get => isNavigating; set => isNavigating = value; }
    public Vector3 PointToFollow { get => pointToFollow; set => pointToFollow = value; }

    private new void Start()
    {
        base.Start();
        navigationAgent = GetComponentInChildren<NavMeshAgent>();
        if (navigationAgent == null ) 
            {
            Debug.Log("EROAREEEEEEEEEEE");
            }
        navigationAgent.updateRotation = false;
    }

    private new void Update()
    {
        if (Time.timeScale == 0f) return;

        animationManager.PlayAnimation(moveDir);

        if (stats.Health <= 0)
        {
            if (!dead)
            {
                StartCoroutine(ExecuteDeath());
            }
        }

        updateToPoint();
        //this.transform.Rotate(new Vector3(45, 0, 0));
        if (isNavigating)
        {
            MoveDir = pointToFollow - gameObject.transform.position;
        }
        else
        {
            MoveDir = Vector3.zero;
        }
    }
    public void updateToPoint()
    {
        //Debug.Log("UPDATE " + gameObject.name);
        try
        {
            navigationAgent.isStopped = !isNavigating;
            navigationAgent.destination = pointToFollow;
                if (isWalking)
                {
                    navigationAgent.speed = speed * walkSlowdown;
                }
                else
                {
                    navigationAgent.speed = speed;
                } 
        }
        catch 
        {
            Debug.LogWarning("Eroare la AI inamic " + gameObject.name);
        }
    }

}
