using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera fpsCam;

    public PatternRecoil recoil;

    public ParticleSystem muzzleFlash;

    private float nextRoundToFire = 0.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= nextRoundToFire)
        {
            nextRoundToFire = Time.time + 1f/fireRate;
            Shoot();
        }

    }
    void Shoot()
    {
        recoil.startRecoil();
        muzzleFlash.Play();
        RaycastHit hit;
        // This function returns true or false, wether an object got hit or not.
        // "out hit" stores all the info about the object that the ray interacted with in the variable
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
