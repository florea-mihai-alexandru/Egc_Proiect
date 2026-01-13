using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public PlayerStats playerstats;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerstats.Heal(playerstats.MaxHealth);
            Debug.Log("A luat heal");
        }
    }
}
