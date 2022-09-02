using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIK;
    public Transform weaponParent;


    Gun weapon;
    public Animator weaponController;

    void Start()
    {
        Gun existingWeapon = GetComponentInChildren<Gun>();
        if(existingWeapon)
        {
            Equip(existingWeapon);
        }
    }


    void Update()
    {
        if(weapon)
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
        else
        {

        }
    }

    public void Equip(Gun newWeapon)
    {
        if(weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon = newWeapon;
        weapon.rayCastDestination = crossHairTarget;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weaponController.Play("equip_" + weapon.weaponName);
        Debug.Log("Equipped a weapon!");
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
