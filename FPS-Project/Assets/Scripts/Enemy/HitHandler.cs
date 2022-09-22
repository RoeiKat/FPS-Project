using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    public EnemyAI enemyAI;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if(health && enemyAI.enabled == true)
        {
            health.TakeDamage(enemyAI.dmg);
        }
    }
}
