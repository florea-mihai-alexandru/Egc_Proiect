using System.Collections;
using Unity.Mathematics;
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

    const float yDirRotation = 60f;
    private Vector3 originalRotation;

    [SerializeField]
    private Transform weaponHolder;

    private void Start()
    {
        originalRotation = weaponHolder.localRotation.eulerAngles;
    }


    private void Update()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    public void Attack(Vector3 direction)
    {
        HandleRotationOnAttack(direction);

        if (timeBtwAttack > 0)
        {
            return;
        }
        timeBtwAttack = currentWeapon.attackSpeed;

        if (currentWeapon.isRanged)
        {
            ExecuteRangedAttack(direction);
        }
        else
        {
            StartCoroutine(ExecuteMeleeAttack(direction));
        }
    }

    void HandleRotationOnAttack(Vector3 direction)
    {
        Vector3 rotation = originalRotation;
        if (direction == Vector3.forward)
        {
            rotation = new Vector3(originalRotation.x, yDirRotation, originalRotation.z);
            FlipX(weaponHolder, false);
        }
        else if (direction == Vector3.right)
        {
            rotation = originalRotation;
            FlipX(weaponHolder, true);
        }
        else if (direction == Vector3.left)
        {
            rotation = originalRotation;
            FlipX(weaponHolder, false);
        }
        else if (direction == Vector3.back)
        {
            rotation = new Vector3(originalRotation.x, -yDirRotation, originalRotation.z);
            FlipX(weaponHolder, false);
        }
        weaponHolder.rotation = Quaternion.Euler(rotation * transform.lossyScale.x);
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
        float scaleCompensation = transform.lossyScale.x;
        attackPos.localPosition = (direction * currentWeapon.offset);
        attackPos.localPosition = new Vector3(attackPos.localPosition.x / scaleCompensation, attackPos.localPosition.y, attackPos.localPosition.z);

        yield return new WaitForSeconds(currentWeapon.attackSpeed);

        weaponHolder.rotation = Quaternion.Euler(originalRotation);

        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, currentWeapon.attackRange/2, whatIsEnemies);
        foreach (Collider enemy in enemiesToDamage)
        {
            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;

            //if (Vector3.Dot(direction, dirToEnemy) > 0.1f)
            //{
                PlayerStats enemyScript = enemy.GetComponentInChildren<PlayerStats>();
                if (enemyScript != null)
                {
                Debug.Log("TOOK DAMAGE");
                    enemyScript.TakeDamage(currentWeapon.damage);
                }
            //}
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (currentWeapon != null && attackPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPos.position, currentWeapon.attackRange);
        }
    }


    public void FlipX(Transform transform, bool flip)
    { 
            Vector3 scale = transform.localScale;
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