using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepOriginalScale : MonoBehaviour
{
    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = originalScale;
    }
}
