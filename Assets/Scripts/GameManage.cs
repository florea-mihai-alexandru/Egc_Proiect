using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    public static GameManage instance;

    public List<string> unlockedWeaponNames = new List<string>();

    public List<string> collectedPickupIDs = new List<string>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject);         }
    }

    public bool HasWeapon(string weaponName)
    {
        return unlockedWeaponNames.Contains(weaponName);
    }
}
