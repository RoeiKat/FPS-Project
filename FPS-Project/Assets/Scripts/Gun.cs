using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Gun : MonoBehaviour
{
    class Bullet 
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
    }

    Vector3 getPosition(Bullet bullet) 
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity*bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet createBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.time = 0.0f;
        return bullet;
    }

    [Header("---Weapon Settings---")]
    public float damage = 10f;
    public float range = 100f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float fireRate = 15f;
    private float nextRoundToFire = 0.2f;
    public float bulletMaxLifeTime = 3f;
    List <Bullet> bullets = new List<Bullet>(); 


    [Header("---Magazine Settings---")]
    public int bulletsPerMag = 30;
    public int bulletsLeft = 90;
    public int currentBullets;

    // Basic magazine handling;
    // public int maxAmmo = 30;
    // public int currentAmmo;
    // private bool isReloading = false;
    // public float reloadTime = 5f;

    public Camera fpsCam;
    Ray ray;
    RaycastHit hitInfo;
    public Transform rayCastOrigin;
    public Transform rayCastDestination;

    [Header("---AimingDownSights Setting---")]
    // zoomSpeed should be between 1~0, 1 is the fastest
    public float zoomSpeed;
    public float cameraZoomValue;
    private float cameraFOV;
    private float cameraZoomFOV;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public Vector3 aimingDownSightPosition;
    public Quaternion aimDownSightsRotation;
    public float adsSpeed = 3f;

    [Header("---Recoil Settings---")]
    public PatternRecoil patternRecoil;
    [SerializeField] float prefabRecoilZ;
    Vector3 currentPosition; 
    Vector3 targetPosition;

    [Header("---Weapon Effects---")]
    public GameObject wallHitPrefab;
    public ParticleSystem shells;
    [SerializeField] AudioSource shotSound;
    public ParticleSystem muzzleFlash;
    public TrailRenderer tracerEffect;
    
    void Start()
    {
        currentBullets = bulletsPerMag;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        cameraFOV = fpsCam.fieldOfView;
        cameraZoomFOV = fpsCam.fieldOfView - cameraZoomValue;
    }

    void Update()
    {
        updateBullet(Time.deltaTime);
        currentPosition = transform.localPosition;
        aimDownSights();
        if(Input.GetKey(KeyCode.R) && currentBullets < 30)
        {
                startReload();
        }
        if (currentBullets <= 0)
        {
            if(Input.GetKey(KeyCode.R))
            {
                startReload();
            }
            return;
        }
        if(Input.GetButtonDown("Fire1"))
        {
            patternRecoil.resetIndex();
        }
        if(Input.GetButton("Fire1") && Time.time >= nextRoundToFire)
        {
            nextRoundToFire = Time.time + 1f/fireRate;
            fireBullet();
            shotSound.Play();
            muzzleFlash.Play();
            generateRecoil();
            emitShells();
        }
    }

    public void fireBullet()
    {
        currentBullets--;
        patternRecoil.startRecoil();

        Vector3 velocity = (rayCastDestination.position - rayCastOrigin.position).normalized * bulletSpeed;
        var bullet = createBullet(rayCastOrigin.position, velocity);
        bullets.Add(bullet);
    }
    public void updateBullet(float deltaTime)
    {
        simulateBullets(deltaTime);
        destroyBullets();
    }

    private void simulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet => 
        {
            Vector3 p0 = getPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = getPosition(bullet);
            raycastSegment(p0, p1, bullet);
        });
    }

    private void raycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        ray.origin = start;
        ray.direction = direction;
        if(Physics.Raycast(ray, out hitInfo, distance))
        {
            //Effects
            Debug.Log(hitInfo.transform);
            GameObject hitVFX = Instantiate(wallHitPrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
            hitVFX.transform.parent = hitInfo.transform;
            bullet.time = bulletMaxLifeTime;
            bullet.tracer.transform.position = hitInfo.point;
            //Damaging the target
            Health targetHealth = hitInfo.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void destroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= bulletMaxLifeTime);
    }

    private void aimDownSights()
    {
        if(Input.GetButton("Fire2"))
        {
        transform.localPosition = Vector3.Lerp(transform.localPosition, aimingDownSightPosition, adsSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, aimDownSightsRotation, adsSpeed * Time.deltaTime);
        fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView ,cameraZoomFOV, zoomSpeed);
        }
        else
        {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, adsSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, adsSpeed * Time.deltaTime);
        fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, cameraFOV, zoomSpeed);
        }
    }

    private void startReload()
    {
        if(bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }

    
    void generateRecoil()
    {
        targetPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - prefabRecoilZ);
        transform.localPosition = targetPosition;
    }

    void emitShells()
    {
        shells.Emit(1);
    }
}

    // IEnumerator Reload()
    // {
    //     isReloading = true;

    //     yield return new WaitForSeconds(reloadTime);

    //     isReloading = false;
    //     currentAmmo = maxAmmo;
    // }

    // Old shooting mechanics (without bullet drop)
    // void fireBullet()
    // {
        // currentBullets--;
        // patternRecoil.startRecoil();
        
        // ray.origin = rayCastOrigin.position;
        // ray.direction = rayCastDestination.position - rayCastOrigin.position;

        // // var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        // // tracer.AddPosition(ray.origin);
        // muzzleFlash.Play();
        // generateRecoil();
        // emitShells();

        // This function returns true or false, wether an object got hit or not.
        // "out hit" stores all the info about the object that the ray interacted with in the variables

        // if(Physics.Raycast(ray, out hitInfo, range))
        // {
        //     //Effects
        //     GameObject hitVFX = Instantiate(wallHitPrefab, hitInfo.point, Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
        //     hitVFX.transform.parent = hitInfo.transform;
        //     tracer.transform.position = hitInfo.point;
        //     //Damaging the target
        //     Health targetHealth = hitInfo.transform.GetComponent<Health>();
        //     if (targetHealth != null)
        //     {
        //         targetHealth.TakeDamage(damage);
        //     }
        // }
        
    // }
