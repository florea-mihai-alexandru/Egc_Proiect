using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    public float speed;
    private float nextActionTime = 0.0f;
    public float period = 1f;
    public Rigidbody rb;
    public Vector3[] dirrections = new Vector3[4];
    private int currentDir = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dirrections[0] = new Vector3(1, 0, 0);
        dirrections[1] = new Vector3(0, 0, 1);
        dirrections[2] = new Vector3(-1, 0, 0);
        dirrections[3] = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            currentDir = (currentDir + 1) % 4;
        }
        rb.velocity = speed * dirrections[currentDir];
    }
}
