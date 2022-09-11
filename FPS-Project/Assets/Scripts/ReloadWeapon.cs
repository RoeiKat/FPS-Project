using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{
    public Animator weaponController;
    public WeaponAnimationEvents animationEvents;
    public ActiveWeapon activeWeapon;
    public Transform leftHand;

    GameObject magazineHand;

    // Start is called before the first frame update
    void Start()
    {
        animationEvents.WeaponAnimationEvent.AddListener(onAnimationEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Gun weapon = activeWeapon.getActiveWeapon();
            if(weapon && weapon.currentBullets < weapon.bulletsPerMag)
            {
             weaponController.SetTrigger("reload_weapon");
            }
            else return; 
        }
    }

    void onAnimationEvent(string eventName)
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "cant_shoot":
                DisableShooting();
                break;
            case "detach_magazine":
                DetachMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
            case "can_shoot":
                EnableShooting();
                break;
        }
    }
    void DisableShooting()
    {
        activeWeapon.canShoot = false;
    }
    void DetachMagazine()
    {
        Gun weapon = activeWeapon.getActiveWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);

    }
    void AttachMagazine()
    {
        Gun weapon = activeWeapon.getActiveWeapon();
        weapon.startReload();
    }
    void EnableShooting()
    {
        activeWeapon.canShoot = true;
    }
}
