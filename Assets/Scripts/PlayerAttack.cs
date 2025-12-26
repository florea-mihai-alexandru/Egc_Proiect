using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    private void Update()
    {
        // 1. Sc?dem timpul în fiecare frame, indiferent de orice altceva
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }

        // 2. Verific?m ap?sarea tastei
        if (Input.GetKey(KeyCode.Space))
        {
            // 3. Atac? DOAR dac? timpul a expirat
            if (timeBtwAttack <= 0)
            {
                ExecuteAttack();
                // 4. Reset?m timpul de a?teptare imediat dup? atac
                timeBtwAttack = startTimeBtwAttack;
            }
        }
    }

    void ExecuteAttack()
    {
        Debug.Log("Atac executat !!!");
        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, whatIsEnemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            Enemy enemyScript = enemiesToDamage[i].GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
