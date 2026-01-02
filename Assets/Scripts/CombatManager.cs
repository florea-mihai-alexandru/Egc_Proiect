using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponData currentWeapon;

    private float timeBtwAttack;

    [Header("Detection Settings")]
    public Transform attackPos;
    public LayerMask whatIsEnemies;

    public UnityEvent<Vector3> attackAnimEvent;

    private void Update()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void Attack(Vector3 direction)
    {
        if (timeBtwAttack > 0)
        {
            return;
        }
        timeBtwAttack = currentWeapon.attackSpeed;
        Debug.Log("Atac cu " + currentWeapon.weaponName);

        if (currentWeapon.isRanged)
        {
            ExecuteRangedAttack(direction);
        }
        else
        {
            StartCoroutine(ExecuteMeleeAttack(direction));
        }

        attackAnimEvent?.Invoke(direction);
    }

    void ExecuteRangedAttack(Vector3 direction)
    {
        attackPos.localPosition = new Vector3(0, attackPos.localPosition.y, 0);
        if (currentWeapon.projectilePrefab != null)
        {
            GameObject bullet = Instantiate(currentWeapon.projectilePrefab, attackPos.position, Quaternion.identity);
            Projectile projScript = bullet.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Setup(direction, currentWeapon.projectileSpeed, currentWeapon.damage);
            }
        }
        else
        {
            Debug.LogWarning("Lipseste Prefab-ul proiectilului pe arma: " + currentWeapon.weaponName);
        }
    }

    IEnumerator ExecuteMeleeAttack(Vector3 direction)
    {
        yield return new WaitForSeconds(currentWeapon.attackSpeed);
        float scaleCompensation = transform.lossyScale.x;
        attackPos.localPosition = (direction * currentWeapon.offset) / scaleCompensation;
        
        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, currentWeapon.attackRange, whatIsEnemies);

        foreach (Collider enemy in enemiesToDamage)
        {
            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;

            //if (Vector3.Dot(direction, dirToEnemy) > 0.1f)
            //{
                PlayerStats enemyScript = enemy.GetComponentInChildren<PlayerStats>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(currentWeapon.damage);
                }
            //}
        }
        Debug.Log(currentWeapon.name + " " + currentWeapon.damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (currentWeapon != null && attackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, currentWeapon.attackRange);
        }
    }
}