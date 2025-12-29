using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    public PlayerAttack playerAttack;
    public WeaponData[] allWeapons;

    public Transform weaponHolderHUD;
    public Transform weaponHolderPlayer;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Q) )
        {
            selectedWeapon--;
            if ( selectedWeapon < 0 )
            { 
                selectedWeapon = allWeapons.Length - 1;
            }
            SelectWeapon();
        }
        if ( Input.GetKeyDown(KeyCode.E) )
        {
            selectedWeapon++;
            if ( selectedWeapon >= allWeapons.Length )
            { 
                selectedWeapon = 0;
            }
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        //Sincronizare date atac
        if (playerAttack != null && allWeapons.Length > selectedWeapon)
        {
            playerAttack.currentWeapon = allWeapons[selectedWeapon];
        }

        //Control vizual HUD (folosind weaponHolderHUD)
        int i = 0;
        foreach (Transform weapon in weaponHolderHUD)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }

        //Control vizual Player (folosind weaponHolderPlayer)
        if (weaponHolderPlayer != null)
        {
            int j = 0;
            foreach (Transform weapon in weaponHolderPlayer)
            {
                weapon.gameObject.SetActive(j == selectedWeapon);
                j++;
            }
        }
    }
}
