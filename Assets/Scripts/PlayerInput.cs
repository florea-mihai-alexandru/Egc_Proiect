using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> OnMovementInput;
    public UnityEvent<Vector3> OnAttack;

    private void Update()
    {
        MovementInputCheck();
        AttackInputCheck();
    }

    private void MovementInputCheck()
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.D)) x = 1;
        else if (Input.GetKey(KeyCode.A)) x = -1;

        if (Input.GetKey(KeyCode.W)) y = 1;
        else if (Input.GetKey(KeyCode.S)) y = -1;
        Vector3 moveDir = new Vector3(x, 0, y).normalized;

        OnMovementInput?.Invoke(moveDir);
    }

    private void AttackInputCheck()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow)) dir = new Vector3(0, 0, 1);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) dir = new Vector3(0, 0, -1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) dir = new Vector3(-1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) dir = new Vector3(1, 0, 0);

        if (dir != Vector3.zero)
        {
            OnAttack?.Invoke(dir);
        }
    }
}
