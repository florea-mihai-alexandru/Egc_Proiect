using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float destroyIn = 3f;
    private float aliveFor = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aliveFor += Time.deltaTime;
        if (aliveFor > destroyIn)
        {
            Destroy(gameObject);
        }
    }
}
