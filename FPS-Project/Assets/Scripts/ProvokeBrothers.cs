using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvokeBrothers : MonoBehaviour
{
    EnemyAI enemyAI;
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyAI.isProvoked)
        {

        }
    }

    private void provoke()
    {
        foreach (Transform child in transform)
        {
            EnemyAI enemyBrotherAI = child.GetComponent<EnemyAI>();
            enemyBrotherAI.isProvoked = true;
        }
    }
}
