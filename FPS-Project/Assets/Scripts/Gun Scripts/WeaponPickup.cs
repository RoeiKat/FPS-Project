using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Gun weaponPrefab;
    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        if(activeWeapon)
        {
            Gun newWeapon = Instantiate(weaponPrefab);
            StartCoroutine(activeWeapon.Equip(newWeapon));
        }
    }
}
