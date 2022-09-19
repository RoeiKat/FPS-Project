using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target;
    [SerializeField] float turnSpeed;
    Animator animator;
    public float dmg = 10f;


    public float chaseRange = 10f;
    float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    public bool stopMovement = false;
    public bool isAttacking = false;
    public bool startScream = false;
    public Transform hitCheck;

    [Header("Sounds")]
    public AudioSource sfx;
    public AudioClip idleSound, attackSound, screamSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        sfx.Play();
    }

    void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(isProvoked)
        {
            if(startScream)
            {
                faceTarget();
                StartCoroutine(zombieScream());
            }
            else 
            {
                engageTarget();
            }
        }
        else if(distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    private void engageTarget()
    {
        faceTarget();
        if(distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            if(!isAttacking && !stopMovement)
            {
             chaseTarget();
            }
        }   
        if(distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            if(!isAttacking)
            {
                StartCoroutine(attackTarget());
            }
        }
        else
        {
            animator.SetBool("attack", false);
        }

    }

    public void damageTaken()
    {
        if(isProvoked)
        {
            return;
        }
        else
        {
            isProvoked = true;
        }
    }
    
    private void chaseTarget()
    {
        if(sfx.clip != screamSound)
        {
            sfx.clip = screamSound;
            sfx.Play();
        }
        navMeshAgent.SetDestination(target.position);
        hitCheck.GetComponent<HitHandler>().enabled = false;
    }
    
    // private void attackTarget()
    // {
    //     Debug.Log("Attacking the player");
    //     animator.SetBool("attack", true);
    //     hitCheck.GetComponent<HitHandler>().enabled = true;
    // }

    IEnumerator attackTarget()
    {
        isAttacking = true;
        stopMovement = true;
        sfx.clip = attackSound;
        sfx.Play();
        animator.SetBool("attack",true);
        hitCheck.GetComponent<HitHandler>().enabled = true;
        do
        {
            yield return new WaitForEndOfFrame();
        } 
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        isAttacking = false;
        stopMovement = false;
    }

    private void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    IEnumerator zombieScream()
    { 
        startScream = false;
        stopMovement = true;
        Debug.Log("Screaming");
        sfx.clip = screamSound;
        sfx.Play();
        animator.SetBool("scream", true);
        do
        {
            yield return new WaitForEndOfFrame();
        } 
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        animator.SetBool("scream", false);
        stopMovement = false;
        engageTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,chaseRange);
    }
}
