using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;
    public string uniqueID;

    void Update()
    {
        float newY = Mathf.Sin(Time.time * 5f) * 0.5f;
        transform.position = new Vector3(transform.position.x, transform.position.y + (newY * Time.deltaTime), transform.position.z);
        //transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }

    void Start()
    {
        if (GameManage.instance != null)
        {
            if (GameManage.instance.collectedPickupIDs.Contains(uniqueID))
            {
                Destroy(gameObject); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coliziune detactata cu: " + other.name);
        if (other.CompareTag("Player"))
        {
            HUD_WeaponSwitch inventory = GameObject.FindObjectOfType<HUD_WeaponSwitch>();
            if (inventory != null)
            {
                inventory.PickUpWeapon(weaponData);
                if (GameManage.instance != null)
                {
                    if (!GameManage.instance.collectedPickupIDs.Contains(uniqueID))
                    {
                        GameManage.instance.collectedPickupIDs.Add(uniqueID);
                    }
                }
                Destroy(gameObject);
                Debug.Log("Arma a fost ridicata");
            }
        }
    }
}
