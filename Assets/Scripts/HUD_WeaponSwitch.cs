using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    [Header("Referințe Scripturi")]
    [SerializeField] public CombatManager playerAttack;
    [SerializeField] public AnimationManager animationManager;

    [Header("Configurare Arme")]
    public WeaponData[] allWeapons;

    [Header("Referințe Vizuale")]
    [SerializeField] public Transform weaponHolderHUD;
    [SerializeField] public Transform weaponHolderPlayer;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
            {
                selectedWeapon = allWeapons.Length - 1;
            }
            SelectWeapon();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedWeapon++;
            if (selectedWeapon >= allWeapons.Length)
            {
                selectedWeapon = 0;
            }
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        if (playerAttack != null && allWeapons.Length > selectedWeapon)
        {
            playerAttack.currentWeapon = allWeapons[selectedWeapon];

            if (animationManager != null)
                animationManager.CurWeaponIndex = selectedWeapon;
        }

        if (weaponHolderHUD != null)
        {
            int i = 0;
            foreach (Transform weapon in weaponHolderHUD)
            {
                weapon.gameObject.SetActive(i == selectedWeapon);
                i++;
            }
        }

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