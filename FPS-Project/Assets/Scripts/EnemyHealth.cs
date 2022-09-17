using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    EnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        enemyAI.damageTaken();
        if(health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
