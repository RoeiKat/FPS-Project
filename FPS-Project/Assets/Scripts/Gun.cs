using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public int maxAmmo = 30;
    public int currentAmmo;
    private bool isReloading = false;
    public float reloadTime = 5f;

    public Camera fpsCam;

    public PatternRecoil recoil;

    public GameObject wallHitPrefab;
    public Transform spawnAtRuntime;

    public ParticleSystem shells;

    [SerializeField] AudioSource shotSound;
    
    public ParticleSystem muzzleFlash;

    private float nextRoundToFire = 0.2f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R) && currentAmmo < 30)
        {
                 StartCoroutine(Reload());
        }
        if (currentAmmo <= 0)
        {
            if(Input.GetKey(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
            return;
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
        currentAmmo--;
        recoil.startRecoil();
        muzzleFlash.Play();
        emitShells();
        RaycastHit hit;
        // This function returns true or false, wether an object got hit or not.
        // "out hit" stores all the info about the object that the ray interacted with in the variable
        Debug.Log(fpsCam.transform.forward);
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject hitVFX = Instantiate(wallHitPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            hitVFX.transform.parent = spawnAtRuntime;
            Debug.Log(hit.transform.name);
        }
        
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        currentAmmo = maxAmmo;
    }

    void emitShells()
    {
        shells.Emit(1);
    }
}
