using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour
{
    public EnemyAI enemyAI;
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if(health)
        {
            health.TakeDamage(enemyAI.dmg);
        }
    }
}
