using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {

    public float speed;
    private float waitTime;
    public float startWaitTime;

    //find player
    private Transform playerPos;

    public Transform[] moveSpots;
    private int randomSpot;

    //set "chase" area
    public float lookRadius = 15f;

    private NavMeshAgent agent;

    //audio variables
    private AudioSource source;
    public AudioClip panic;
    public AudioClip ambient;
    public AudioClip notice;
    private int sound;
    private bool alreadyPlayed;
    private float delay;
    

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);

        agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //track distance
        float distance = Vector3.Distance(playerPos.position, transform.position);
        //in "chase" or patrol
        if (distance <= lookRadius)
        {
            agent.SetDestination(playerPos.position);
            sound = 2;
            Sounds();

            if (distance <= agent.stoppingDistance)
            {
                //pounce
                FacePlayer();
            }
        }
        else
        {
            Cycle();
            alreadyPlayed = false;
        }
    }


    //walk around
    void Cycle()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < .02f)
        {
            randomSpot = Random.Range(0, moveSpots.Length);
            waitTime = startWaitTime;
            source.PlayOneShot(ambient);
            Sounds();
        }
        else
        {
            waitTime -= Time.deltaTime;
            Transform target = moveSpots[randomSpot];
            Vector3 rDirection = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(rDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    //look at me you coward
    void FacePlayer()
    {
        Vector3 direction = (playerPos.position - transform.position).normalized;
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

    //sound function
    void Sounds()
    {
        if (sound == 1 && alreadyPlayed == false)
        {
            source.PlayOneShot(panic);
            alreadyPlayed = true;
        }
        else if (sound == 2 && alreadyPlayed == false)
        {
            source.PlayOneShot(notice);
            alreadyPlayed = true;
        }
    }
}
