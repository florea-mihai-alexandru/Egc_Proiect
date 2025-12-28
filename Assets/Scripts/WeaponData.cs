using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Combat/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float attackSpeed;
    public float attackRange;
    public float offset;
    public GameObject weaponModel; // Pentru a schimba vizual arma

    public bool isRanged;
    public GameObject projectilePrefab;
    public float projectileSpeed;
}