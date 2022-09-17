using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Transform target;
    [SerializeField] float turnSpeed;
    Animator animator;


    public float chaseRange = 10f;
    float distanceToTarget = Mathf.Infinity;
    public bool isProvoked = false;
    public bool startScream = false;
    public Transform hitCheck;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            chaseTarget();
        }   
        if(distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            attackTarget();
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
        navMeshAgent.SetDestination(target.position);
        hitCheck.GetComponent<HitHandler>().enabled = false;
    }
    
    private void attackTarget()
    {
        Debug.Log("Attacking the player");
        animator.SetBool("attack", true);
        hitCheck.GetComponent<HitHandler>().enabled = true;
    }

    private void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    IEnumerator zombieScream()
    {
        animator.SetBool("scream", true);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        animator.SetBool("scream", false);
        startScream = false;
        engageTarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,chaseRange);
    }
}
