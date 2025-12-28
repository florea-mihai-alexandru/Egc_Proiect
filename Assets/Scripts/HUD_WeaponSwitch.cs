using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Q) )
        {
            selectedWeapon--;
            if ( selectedWeapon < 0 )
            { 
                selectedWeapon = transform.childCount-1;
            }
            SelectWeapon();
        }
        if ( Input.GetKeyDown(KeyCode.E) )
        {
            selectedWeapon++;
            if ( selectedWeapon >= transform.childCount )
            { 
                selectedWeapon = 0;
            }
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
                i++;
        }
    }
}
