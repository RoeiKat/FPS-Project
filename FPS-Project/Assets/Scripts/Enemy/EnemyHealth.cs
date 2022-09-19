using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject player;
    public float health = 100f;
    public float destroyTimer = 10f;
    public float crawlHealth = 20f;
    public bool crawlingZombie = false;
    private bool hasCrawled = false;
    public bool isDead = false;    
    EnemyAI enemyAI;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        enemyAI.damageTaken();
        if(health <= 0)
        {
            die();
        }
    }
    void die()
    {
        isDead = true;
        int randomDeath = Random.Range(1,2);
        int randomCrawl = Random.Range(1,2);
        if (randomCrawl == 2 ) crawlingZombie = true;
        if(isDead)
        {
            if(crawlingZombie && randomDeath == 2 && !hasCrawled)
            {
                    StartCoroutine(startCrawl());
                    return;
            }
            if(hasCrawled)
            {
                enemyAI.enabled = false;
                enemyAI.sfx.Stop();
                Destroy(gameObject, destroyTimer);
                animator.SetTrigger("dead");
                return;
            }
            else
            {
                StartCoroutine(deathAnimationHandler(randomDeath));
            }
        }
    }
    IEnumerator deathAnimationHandler(int deathIndex)
    {
        enemyAI.enabled = false;
        enemyAI.stopMovement = true;
        enemyAI.navMeshAgent.speed = 0f;
        enemyAI.sfx.Stop();
        animator.Play("zombie_death" + deathIndex);
        do
        {
            yield return new WaitForEndOfFrame();
        } 
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        Destroy(gameObject, destroyTimer);
    }


    IEnumerator startCrawl()
    {
        enemyAI.enabled = false;
        animator.Play("zombie_death2");
        do
        {
            yield return new WaitForEndOfFrame();
        } 
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        enemyAI.enabled = true;
        enemyAI.stopMovement = true;
        hasCrawled = true;
        isDead = false;
        health  = crawlHealth;
        enemyAI.navMeshAgent.speed = 1f;
        animator.SetTrigger("crawl");
        enemyAI.stopMovement = false;
        enemyAI.sfx.clip = enemyAI.screamSound;
        enemyAI.sfx.Play();
    }
    
    public void dealDamage(float dmg)
    {
        dmg = enemyAI.dmg;
        Health playerHealth = player.GetComponent<Health>();
        playerHealth.TakeDamage(dmg);
    }
}
