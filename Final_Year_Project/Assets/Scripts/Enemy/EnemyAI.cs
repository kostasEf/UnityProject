using UnityEngine;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;                          // The patrolling speed of the enemy.
    public float chaseSpeed = 5f;                           // The chasing speed of the enemy.
    public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
    public Transform[] patrolPoints;                     // An array of transforms for the patrol route.
    private EnemySight enemySight;                          // Reference to the EnemySight script.
	public Transform[] coverPoints;                         // An array of transforms for the cover positions.
    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	Vector3[] searchPoints;
    private Transform player;                               // Reference to the player's transform.
	float[] distances;
    private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	public bool aimEnemy;
    private PlayerPosition playerPosition;          // Reference to the last global sighting of the player.
	public bool search = false;
    private float patrolTimer;                              // A timer for the patrolWaitTime.
	public bool die= false;
    public int wayPointCounter, counter;  					// A counter for the way point array.
	private bool enemyDead;
    private Animator anim;                     		        // Reference to the animator component.
	private HashIDs hash; 
	public float health = 100f;
	
	private float timer;
    
    void Awake ()
    {
        enemySight = GetComponent<EnemySight>();
		distances  = new float[12];
        nav = GetComponent<NavMeshAgent>();
		searchPoints  = new Vector3[4];
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerPosition>();
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
    }
    
    
    void Update ()
    {
		
        if(health <= 0)
		{
			die = true;
		}
		
		
		Crouch();
		
		if(die == false && aimEnemy == false )
		{
			// If the player is visible and has more than 0 health...
        	if(enemySight.playerInSight && playerHealth.health > 0f){
            	// ... shoot.
            	Shooting();
			}
        	else if(enemySight.personalLastSighting != playerPosition.resetPosition && playerHealth.health > 0f){
            	
				if(search == false)
				{
					Chase();
				}else{
            		Search();
				}
				
			}
        	else{
            	Patrolling();
				counter = 0;
				search = false;
			}
		}else if(die == true){
			Dying();
		}
			
    }
    
	
	void Crouch()
	{
	
		anim.SetBool(hash.crouchBool, aimEnemy);
		
		if(anim.GetBool(hash.crouchBool))
		{
			gameObject.GetComponent<EnemyShooting>().enabled = false;
			int number = UnityEngine.Random.Range (3, 10);
			StandTimer(number);		
		}else{
			gameObject.GetComponent<EnemyShooting>().enabled = true;
		}
		
	}
	
	
	void StandTimer(int timeToStand)
	{
		timer += Time.deltaTime;
		//int timeToStand = Random.Range(0,10);
		if(timer >= timeToStand)
		{
			aimEnemy = false;
			timer = 0;
		}
	}
    
    void Shooting ()
    {
        nav.Stop();
		counter = 0;
		search = false;
    }
    
    
    void Chase ()
    {
        // Create a vector from the enemy to the last sighting of the player.
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        
        // If the the last personal sighting of the player is not close...
        if(sightingDeltaPos.sqrMagnitude > 4f )
            // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
            nav.destination = enemySight.personalLastSighting;
        
        // Set the appropriate speed for the NavMeshAgent.
        nav.speed = chaseSpeed;
        
        if(nav.remainingDistance < nav.stoppingDistance)
		{
			search = true;
		}
        
    }
	
	
	void Search()
	{
		
		
		if(enemySight.playerInSight)
		{
			search = false;
			counter = 0;
			nav.Stop();
		}
		
		for(int i = 0; i < coverPoints.Length; i++)
		{
			distances[i] = Vector3.Distance(enemySight.personalLastSighting, coverPoints[i].position);
			
		}
		
		Array.Sort(distances);
		
		for(int i = 0; i < coverPoints.Length; i++)
		{
			
			for(int j = 0; j < 4; j++)
			{
				if( distances[j] == Vector3.Distance(enemySight.personalLastSighting, coverPoints[i].position) )
				{
					searchPoints[j] = coverPoints[i].position;
				}
			}
			
		}
		
		if( nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            
            if(patrolTimer >= patrolWaitTime)
            {
                if(counter == 4){
					counter = 0;
					search = false;
					if( nav.remainingDistance < nav.stoppingDistance)
        			{
						playerPosition.position = playerPosition.resetPosition;
                		enemySight.personalLastSighting = playerPosition.resetPosition;
					}
					
				}
                else{
                    counter++;
				}
                patrolTimer = 0;
            }
        }
        else{
            patrolTimer = 0;
		}
        nav.destination = searchPoints[counter];
		
	}
	
	
    
    void Patrolling ()
    {
        // Set an appropriate speed for the NavMeshAgent.
        nav.speed = patrolSpeed;
        
        // If near the next waypoint or there is no destination...
        if(nav.destination == playerPosition.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            // ... increment the timer.
            patrolTimer += Time.deltaTime;
            
            // If the timer exceeds the wait time...
            if(patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointCounter.
                if(wayPointCounter == patrolPoints.Length - 1)
                    wayPointCounter = 0;
                else
                    wayPointCounter++;
                
                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        
        // Set the destination to the patrolWayPoint.
        nav.destination = patrolPoints[wayPointCounter].position;
    }
	
	
	void Dying ()
    {
        // The player is now dead.
        enemyDead = true;
        
        // Set the animator's dead parameter to true also.
        anim.SetBool(hash.deadBool, enemyDead);
		gameObject.GetComponent<SphereCollider>().enabled = false;
		gameObject.GetComponent<EnemySight>().enabled = false;
		gameObject.GetComponent<EnemyShooting>().enabled = false;
		gameObject.GetComponent<EnemyAI>().enabled = false;
		
		
		playerPosition.position = playerPosition.resetPosition;
		Destroy(gameObject, 5f);
        
    }
    
    
	public void EnemyDamage(float amount)
	{
		health -= amount;
		playerPosition.position = player.transform.position;
	}
}