using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public int bulletsPerMag = 30;
    public int bulletsLeft = 90;
    public int currentBullets;

    // public int maxAmmo = 30;
    // public int currentAmmo;
    // private bool isReloading = false;
    // public float reloadTime = 5f;

    public Camera fpsCam;
    public Transform shootingPoint;

    private Vector3 originalPosition;
    public Vector3 aimingDownSightPosition;
    public float adsSpeed = 3f;

    public PatternRecoil patternRecoil;
    [SerializeField] float prefabRecoilZ;
    Vector3 currentPosition; 
    Vector3 targetPosition;

    public GameObject wallHitPrefab;

    public ParticleSystem shells;

    [SerializeField] AudioSource shotSound;
    
    public ParticleSystem muzzleFlash;

    private float nextRoundToFire = 0.2f;


    void Start()
    {
        currentBullets = bulletsPerMag;
        originalPosition = transform.localPosition;
        patternRecoil = gameObject.GetComponentInParent<PatternRecoil>();
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.localPosition;
        aimDownSights();
        if(Input.GetKey(KeyCode.R) && currentBullets < 30)
        {
                //  StartCoroutine(Reload());
                startReload();
        }
        if (currentBullets <= 0)
        {
            if(Input.GetKey(KeyCode.R))
            {
                // StartCoroutine(Reload());
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
            Shoot();
            shotSound.Play();
        }
    }
    void Shoot()
    {
        currentBullets--;
        
        patternRecoil.startRecoil();
        muzzleFlash.Play();
        generateRecoil();
        emitShells();
        RaycastHit hit;
        // This function returns true or false, wether an object got hit or not.
        // "out hit" stores all the info about the object that the ray interacted with in the variables
        if(Physics.Raycast(shootingPoint.transform.position, shootingPoint.transform.forward, out hit, range))
        {
            GameObject hitVFX = Instantiate(wallHitPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            hitVFX.transform.parent = hit.transform;
            //Damaging the target
            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
        
    }

    private void aimDownSights()
    {
        if(Input.GetButton("Fire2"))
        {
        transform.localPosition = Vector3.Lerp(transform.localPosition, aimingDownSightPosition, adsSpeed * Time.deltaTime);
        }
        else
        {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, adsSpeed * Time.deltaTime);
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

    // IEnumerator Reload()
    // {
    //     isReloading = true;

    //     yield return new WaitForSeconds(reloadTime);

    //     isReloading = false;
    //     currentAmmo = maxAmmo;
    // }
    
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
