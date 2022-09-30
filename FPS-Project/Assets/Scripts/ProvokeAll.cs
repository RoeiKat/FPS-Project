using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvokeAll : MonoBehaviour
{
    public GameObject[] enemyGroups;
    private void OnTriggerEnter(Collider other)
    {
        provokeAllOnHit();
    }
    
    public void provokeAllOnHit()
    {
        for(int i = 0; i < enemyGroups.Length; i++)
        {
            foreach (Transform child in enemyGroups[i].transform)
            {
                EnemyAI enemyAI = child.GetComponent<EnemyAI>();
                if(enemyAI == null ) return;
                enemyAI.isProvoked = true;
            }
        }
    }
}
