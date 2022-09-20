using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPrefabRecoil : MonoBehaviour
{
    private Vector3 currentRotation, targetRotation;
    // public Transform cam;

    public float recoilX;    
    public float recoilY;    
    public float recoilZ; 

    public float lowerWhileAds = 1.5f;
    ActiveWeapon activeWeapon;

    public float snappiness;
    public float returnSpeed;

    void Start()
    {
        activeWeapon = GetComponentInParent<ActiveWeapon>();
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    
    public void generateRecoil()
    {
        if(!activeWeapon.adsWeapon)
        {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
        if(activeWeapon.adsWeapon)
        {
        targetRotation += new Vector3(recoilX / lowerWhileAds, Random.Range(-recoilY / lowerWhileAds, recoilY * lowerWhileAds), Random.Range(-recoilZ / lowerWhileAds, recoilZ / lowerWhileAds));
        }
    }

}
