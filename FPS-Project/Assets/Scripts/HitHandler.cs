using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    public float dmg = 10f;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if(health)
        {
            health.TakeDamage(dmg);
        }
    }
}
