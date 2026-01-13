using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HUD_WeaponSwitch : MonoBehaviour
{
    [Header("Setări Inventar")]
    public int selectedWeapon = 0;
    [SerializeField]
    public CombatManager playerAttack;

    public List<WeaponData> allWeapons = new List<WeaponData>();

    public List<WeaponData> weaponLibrary = new List<WeaponData>();

    [Header("Referințe Vizuale")]
    [SerializeField]
    public Transform weaponHolderHUD;
    [SerializeField]
    public Transform weaponHolderPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            weaponHolderPlayer = playerObj.transform.Find("WeaponHolder");
            playerAttack = playerObj.GetComponent<CombatManager>();
        }

        if (GameManage.instance != null)
        {
            if (GameManage.instance.unlockedWeaponNames.Count > 0)
            {
                allWeapons.Clear();
                foreach (string savedName in GameManage.instance.unlockedWeaponNames)
                {
                    foreach (WeaponData data in weaponLibrary)
                    {
                        if (data.weaponName == savedName)
                        {
                            allWeapons.Add(data);
                            break;                        }
                    }
                }
            }
        }

        if (allWeapons.Count == 0 && weaponLibrary.Count > 0)
        {
            WeaponData defaultWeapon = weaponLibrary[0]; 
            allWeapons.Add(defaultWeapon);

            if (GameManage.instance != null)
            {
                if (!GameManage.instance.unlockedWeaponNames.Contains(defaultWeapon.weaponName))
                {
                    GameManage.instance.unlockedWeaponNames.Add(defaultWeapon.weaponName);
                }
            }
        }

        SelectWeapon();
    }

    public void PickUpWeapon(WeaponData weapon)
    {
        if (!allWeapons.Contains(weapon))
        {
            allWeapons.Add(weapon);

            if (GameManage.instance != null)
            {
                if (!GameManage.instance.unlockedWeaponNames.Contains(weapon.weaponName))
                {
                    GameManage.instance.unlockedWeaponNames.Add(weapon.weaponName);
                }
            }

            selectedWeapon = allWeapons.Count - 1;
            SelectWeapon();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;
        if ( Input.GetKeyDown(KeyCode.Q) && allWeapons.Count > 1)
        {
            selectedWeapon--;
            if ( selectedWeapon < 0 )
            { 
                selectedWeapon = allWeapons.Count - 1;
            }
            SelectWeapon();
        }
        if ( Input.GetKeyDown(KeyCode.E) && allWeapons.Count > 1)
        {
            selectedWeapon++;
            if ( selectedWeapon >= allWeapons.Count )
            { 
                selectedWeapon = 0;
            }
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        if (allWeapons.Count == 0) return;

        if (selectedWeapon >= allWeapons.Count) selectedWeapon = 0;

        WeaponData currentData = allWeapons[selectedWeapon];

        if (playerAttack != null) playerAttack.currentWeapon = currentData;

        UpdateVisuals(weaponHolderHUD, currentData.weaponName);

        if (weaponHolderPlayer != null)
        {
            UpdateVisuals(weaponHolderPlayer, currentData.weaponName);
        }
    }

    void UpdateVisuals(Transform holder, string activeName)
    {
        if (holder == null) return;

        foreach (Transform child in holder)
        {
            child.gameObject.SetActive(child.name == activeName);
        }
    }
}
