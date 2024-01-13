using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform Player;

    public LayerMask WhatIsGround,WhatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    public float health = 100;
    bool alreadyAttacked;
    Quaternion attackAngle;
    private RaycastHit rayHit;
    public Transform rayOrigin;
    public float sightRange,attackRange;
    public float attackDamage;    
    public bool playerInSightRange,playerInAttackRange;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;

        agent = GetComponent<NavMeshAgent>();
    } 
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position,sightRange,WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position,attackRange,WhatIsPlayer);

        if(!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInSightRange && playerInAttackRange) AttackPlayer();

        drawShit();
    }

    public void TakeDamage(float gDamage)
    {
        health -= gDamage;
        if (0 >= health){
            Destroy(gameObject);
        }
    }
    
    private void Patroling()
    {
        if(!walkPointSet) searchWalkPoint();

        if(walkPointSet == true)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWP = transform.position - walkPoint;

        if(distanceToWP.magnitude < 1f)
            walkPointSet = false;
    }

    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange,walkPointRange);
        float randomX = Random.Range(-walkPointRange,walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX,transform.position.y,transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint,-transform.up,2f,WhatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(Player);

        if(!alreadyAttacked){
            if(playerInAttackRange == true)
            {
                Player.GetComponent<pTakeDmg>().pTakeDamg(attackDamage);
            }

            alreadyAttacked = true;
            Invoke(nameof(resetAttack),timeBetweenAttacks);
        }
    }

    private void resetAttack()
    {
        alreadyAttacked = false;
    }

    private void drawShit()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,sightRange);*/
    }
}
