using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleRandomFly : MonoBehaviour
{
    private Vector3 randomDir;
    private Rigidbody rb;

    public float explosionForce = 600f;
    public float radius = 4f;

    // Start is called before the first frame update
    void Start()
    {
        randomDir = math.abs(UnityEngine.Random.onUnitSphere);
        rb = GetComponent<Rigidbody>();
        //rb.velocity = randomDir * 10;
        rb.AddExplosionForce(explosionForce, transform.position, radius);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
