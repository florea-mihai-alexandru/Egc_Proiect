using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector3 direction;

    public void Setup(Vector3 dir, float spd, float dmg)
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
        if (other.CompareTag("Player")) return;
        // Cautam componenta Enemy pe orice obiect atingem
        PlayerStats enemyScript = other.GetComponent<PlayerStats>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
        Destroy(gameObject); 
    }
}
