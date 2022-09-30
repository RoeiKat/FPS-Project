using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject player;
    public float health = 100f;
    public float destroyTimer = 10f;
    public float crawlHealth = 20f;
    public int randomDeath;
    public bool crawlingZombie = false;
    public float timeTillCrawl = 3f;
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
        if(health <= 0 && isDead == false)
        {
            die();
        }
    }
    void die()
    {
        isDead = true;
        randomDeath = Random.Range(1,8);
        if (randomDeath == 1 || randomDeath == 2 || randomDeath == 3 || randomDeath == 4)
        {
            randomDeath = 1;
        }
        if (randomDeath == 5 || randomDeath == 6 || randomDeath == 7 || randomDeath == 8)
        {
            randomDeath = 2;
        }
        if(isDead)
        {
            enemyAI.stopMovement = true;
            enemyAI.navMeshAgent.speed = 0f;
            if(crawlingZombie && !hasCrawled)
            {
                    randomDeath = 2;
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
        enemyAI.sfx.Stop();
        animator.Play("zombie_death" + deathIndex);
        yield return new WaitForSeconds(0);
        Destroy(gameObject, destroyTimer);
    }


    IEnumerator startCrawl()
    {
        enemyAI.enabled = false;
        enemyAI.sfx.Stop();
        animator.Play("zombie_death2");
        yield return new WaitForSeconds(timeTillCrawl);
        enemyAI.enabled = true;
        enemyAI.stopMovement = false;
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
