using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "SOW/Weapon")]
public class WeaponInfo : ScriptableObject
{
    [Header("Weapon Type")]
    public GameObject[] WeaponType;

    public GameObject[] BulletWeapon;
}
