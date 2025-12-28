using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponData currentWeapon;

    private float timeBtwAttack;

    [Header("Detection Settings")]
    public Transform attackPos;
    public LayerMask whatIsEnemies;

    private void Update()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
        }

        if (timeBtwAttack <= 0 && currentWeapon != null)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Attack(new Vector3(0, 0, 1));
        else if (Input.GetKeyDown(KeyCode.DownArrow)) Attack(new Vector3(0, 0, -1));
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) Attack(new Vector3(-1, 0, 0));
        else if (Input.GetKeyDown(KeyCode.RightArrow)) Attack(new Vector3(1, 0, 0));
    }

    void Attack(Vector3 pos)
    {
        timeBtwAttack = currentWeapon.attackSpeed;
        Debug.Log("Atac cu " + currentWeapon.weaponName);

        if (currentWeapon.isRanged)
        {
            ExecuteRangedAttack(pos);
        }
        else
        {
            ExecuteMeleeAttack(pos);
        }
    }

    void ExecuteRangedAttack(Vector3 direction)
    {
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

    void ExecuteMeleeAttack(Vector3 direction)
    {
        float scaleCompensation = transform.lossyScale.x;
        attackPos.localPosition = (direction * currentWeapon.offset) / scaleCompensation;
        
        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, currentWeapon.attackRange, whatIsEnemies);

        foreach (Collider enemy in enemiesToDamage)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(currentWeapon.damage);
            }
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
}