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
    public HUDController hud;

    Gun[] equipped_weapon = new Gun[2];
    int activeWeaponIndex;

    public Animator weaponController;
    public Animator weaponAnimaton;

    public Camera fpsCam;
    public Camera mainCamera;
    private float cameraFOV;
    public int cameraZoom;
    public float zoomSpeed;

    void Start()
    {
        Gun existingWeapon = GetComponentInChildren<Gun>();
        if(existingWeapon)
        {
            StartCoroutine(Equip(existingWeapon));
            // Equip(existingWeapon);
        }
        cameraFOV = fpsCam.fieldOfView;
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

    void Update()
    {
        var weapon = getWeapon(activeWeaponIndex);
        if(weapon && canShoot)
        {
            if(Input.GetButton("Fire2"))
            {
                weaponController.SetBool("ads_weapon", true);
                zoomCamera(fpsCam);
                zoomCamera(mainCamera);
            }
            else 
            {
                weaponController.SetBool("ads_weapon", false);
                unZoomCamera(fpsCam);
                unZoomCamera(mainCamera);
            }
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

        if (Input.GetKeyDown(KeyCode.Alpha1) && canShoot)
        {
            setActiveWeapon(WeaponSlot.Primary);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && canShoot)
        {
            setActiveWeapon(WeaponSlot.Secondary);
        }
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

    public IEnumerator Equip(Gun newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = getWeapon(weaponSlotIndex);
        if(weapon)
        {
            Destroy(newWeapon.gameObject);
            yield break;
            // Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.rayCastDestination = crossHairTarget;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        equipped_weapon[weaponSlotIndex] = weapon;
        setActiveWeapon(newWeapon.weaponSlot);
        weaponController.Play("equip_" + weapon.weaponName);
        weapon.equipSound.Play();
        do
        {
         yield return new WaitForEndOfFrame();
        } while (weaponController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        canShoot = true;
    }

    // public void Equip(Gun newWeapon)
    // {
    //     int weaponSlotIndex = (int)newWeapon.weaponSlot;
    //     var weapon = getWeapon(weaponSlotIndex);
    //     if(weapon)
    //     {
    //         Destroy(newWeapon.gameObject);
    //         return;
    //         // Destroy(weapon.gameObject);
    //     }
    //     weapon = newWeapon;
    //     canShoot = true;
    //     weapon.rayCastDestination = crossHairTarget;
    //     weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
    //     weapon.transform.localPosition = Vector3.zero;
    //     weapon.transform.localRotation = Quaternion.identity;
    //     equipped_weapon[weaponSlotIndex] = weapon;
    //     setActiveWeapon(newWeapon.weaponSlot);
    //     weaponController.Play("equip_" + weapon.weaponName);
    //     weapon.equipSound.Play();
    // }


    void setActiveWeapon(WeaponSlot weaponSlot)
    {
        var weapon = getWeapon((int)weaponSlot);
        hud.updateGun(weapon);
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

    // IEnumerator switchWeapon(int holsterIndex)
    IEnumerator switchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(holsterWeapon(holsterIndex, activateIndex));
        activeWeaponIndex = activateIndex;
    }
        IEnumerator holsterWeapon(int index, int activateIndex)
    {
        var weapon = getWeapon(index);
        if (weapon) 
        {
            hud.updateGun(weapon);
            canShoot = false;
            GameObject weaponMesh = weapon.transform.GetChild(0).gameObject;
            weaponMesh.gameObject.SetActive(false);
            yield return StartCoroutine(activateWeapon(activateIndex));
        }
    }
    IEnumerator activateWeapon(int index)
    {
        var weapon = getWeapon(index);
        if (weapon) 
        {
            hud.updateGun(weapon);
            GameObject weaponMesh = weapon.transform.GetChild(0).gameObject;
            weaponMesh.gameObject.SetActive(true);
            weaponController.Play("equip_" + weapon.weaponName);
            weapon.equipSound.Play();
            do
            {
                yield return new WaitForEndOfFrame();
            } while (weaponController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            canShoot = true;
        }
    }

    private void zoomCamera(Camera camera)
    {
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, cameraZoom, zoomSpeed * Time.deltaTime);
    }
    private void unZoomCamera(Camera camera)
    {
        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, cameraFOV, zoomSpeed * Time.deltaTime);
    }
}
