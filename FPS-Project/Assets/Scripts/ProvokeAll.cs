using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvokeAll : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        provokeAllOnHit();
    }
    
    public void provokeAllOnHit()
    {
        foreach (Transform child in transform)
        {
            EnemyAI enemyAI = child.GetComponent<EnemyAI>();
            enemyAI.isProvoked = true;
        }
    }
}
