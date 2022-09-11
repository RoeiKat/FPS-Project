using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot {
        Primary = 0,
        Secondary = 1
    }
    public Transform crossHairTarget;
    public Transform[] weaponSlots;
    public bool canShoot = false;
    // public UnityEngine.Animations.Rigging.Rig handIK;
    // public Transform weaponParent;


    Gun[] equipped_weapon = new Gun[2];
    int activeWeaponIndex;

    public Animator weaponController;

    void Start()
    {
        Gun existingWeapon = GetComponentInChildren<Gun>();
        if(existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    public Gun getActiveWeapon()
    {
        return getWeapon(activeWeaponIndex);
    }

    Gun getWeapon(int index) 
    {
        if(index < 0 || index >= equipped_weapon.Length)
        {
            return null;
        }
        return equipped_weapon[index];
    }

    private void semiAutoShoot(Gun weapon)
    {
        if(Input.GetButtonDown("Fire1"))
        {
            weapon.patternRecoil.resetIndex();
        }
        if(Input.GetButtonDown("Fire1") && Time.time >= weapon.nextRoundToFire)
        {
            weapon.nextRoundToFire = Time.time + 1f/weapon.fireRate;
            weapon.fireBullet();
        }
    }

    private void automaticShoot(Gun weapon)
    {
        if(Input.GetButtonDown("Fire1"))
        {
            weapon.patternRecoil.resetIndex();
        }
        if(Input.GetButton("Fire1") && Time.time >= weapon.nextRoundToFire)
        {
            weapon.nextRoundToFire = Time.time + 1f/weapon.fireRate;
            weapon.fireBullet();
        }
    }

    void Update()
    {
        var weapon = getWeapon(activeWeaponIndex);
        if(weapon && canShoot)
        {
            int weaponKind = (int)weapon.autoOrSemi;
            if(weaponKind == 0)
            {
                automaticShoot(weapon);
            }
            if(weaponKind == 1)
            {
                semiAutoShoot(weapon);
            }
        }
        else
        {
            canShoot = false;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setActiveWeapon(WeaponSlot.Primary);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setActiveWeapon(WeaponSlot.Secondary);
        }
    }

    public void Equip(Gun newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = getWeapon(weaponSlotIndex);
        if(weapon)
        {
            Destroy(newWeapon.gameObject);
            return;
            // Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        canShoot = true;
        weapon.rayCastDestination = crossHairTarget;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weaponController.Play("equip_" + weapon.weaponName);
        equipped_weapon[weaponSlotIndex] = weapon;
        setActiveWeapon(newWeapon.weaponSlot);
    }


    void setActiveWeapon(WeaponSlot weaponSlot)
    {
        var weapon = getWeapon((int)weaponSlot);
        if(weapon == null)
        {
            return;
        }
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;
        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }
        StartCoroutine(switchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator switchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(holsterWeapon(holsterIndex));
        yield return StartCoroutine(activateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }
    IEnumerator holsterWeapon(int index)
    {
        var weapon = getWeapon(index);
        if (weapon) 
        {
            canShoot = false;
            // weaponController.SetBool("holster_weapon", true);
            weapon.gameObject.SetActive(false);
            yield return new WaitForSeconds(0);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (weaponController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }
    IEnumerator activateWeapon(int index)
    {
        var weapon = getWeapon(index);
        if (weapon) 
        {
            // weaponController.SetBool("holster_weapon", false);
            weapon.gameObject.SetActive(true);
            weaponController.Play("equip_" + weapon.weaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (weaponController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            canShoot = true;
        }
    }




    // How to save functions to be activated inside the inspector
    // [ContextMenu("Save weapon pose")]
    // void saveWeaponPose()
    // {
    //     GameObjectRecorder recorder = new GameObjectRecorder(handsArmature.gameObject);
    //     recorder.BindComponentsOfType<Transform>(weaponParent.gameObject, false);
    //     recorder.BindComponentsOfType<Transform>(weapon.gameObject, false);
    //     recorder.BindComponentsOfType<Transform>(weaponLeftGrip.gameObject, false);
    //     recorder.BindComponentsOfType<Transform>(weaponRightGrip.gameObject, false);
    //     recorder.TakeSnapshot(0.0f);
    //     recorder.SaveToClip(weapon.weaponAnimation);
    //     UnityEditor.AssetDatabase.SaveAssets();
    // }
}
