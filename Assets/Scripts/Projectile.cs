using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private Vector3 direction;

    public void Setup(Vector3 dir, float spd, int dmg)
    {
        this.direction = dir;
        this.speed = spd;
        this.damage = dmg;

        //Se distruge dupa 3 sec sa nu consume memorie
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Cautam componenta Enemy pe orice obiect atingem
        Enemy enemyScript = other.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
