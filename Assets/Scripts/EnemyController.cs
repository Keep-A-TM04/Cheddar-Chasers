using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    //set "chase" area
    public float lookRadius = 10f;

    //link to player
    private Transform target;
    NavMeshAgent agent;

    //link player as target at start of game
    void Start()
    {
        target = PlayerManager.instance.player.transform;

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //track distance
        float distance = Vector3.Distance(target.position, transform.position);
        //in "chase" mode
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //pounce
                FaceTarget();
            }
        }
    }

    //look at me you coward
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //without breaking your neck pls
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    //Visualize "Chase" area
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
