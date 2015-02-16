using UnityEngine;
using System.Collections;
using System;

public class EnemyAI2 : MonoBehaviour
{
    public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
    public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
    public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
    public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
    public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.
    public Transform[] coverPoints;                         // An array of transforms for the cover positions.
    float[] distances;
	Vector3[] searchPoints;
		
    private EnemySight enemySight;                          // Reference to the EnemySight script.
    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
    private Transform player;                               // Reference to the player's transform.
    private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
    private PlayerPosition playerPosition;          // Reference to the last global sighting of the player.
    private float chaseTimer;                               // A timer for the chaseWaitTime.
    private float patrolTimer;                              // A timer for the patrolWaitTime.
    private int wayPointIndex, counter;  							// A counter for the way point array.
	public bool aimEnemy;
	
	private bool enemyDead;
	public bool search = false;
    private Animator anim;                     		        // Reference to the animator component.
	public bool die= false;
	private HashIDs hash; 
	public float health = 100f;
	
	private float timer;
    
    void Awake ()
    {
        // Setting up the references.
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerPosition>();
		
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		
		distances  = new float[12];
		searchPoints  = new Vector3[4];
    }
    
    
    void Update ()
    {
		
        if(health <= 0)
		{
			die = true;
		}
		
		Crouch();
		
		if(die == false && aimEnemy == false)
		{
			// If the player is in sight and is alive...
        	if(enemySight.playerInSight && playerHealth.health > 0f){
            // ... shoot.
            Shooting();
			}
		}else if(die == true){
			Dying();
		}else if(aimEnemy == true){
			//Crouch ();
		}
			
    }
    
	
	void Crouch()
	{
	
		anim.SetBool(hash.crouchBool, aimEnemy);
		
		if(anim.GetBool(hash.crouchBool))
		{
			gameObject.GetComponent<EnemyShooting>().enabled = false;
			StandTimer();		
		}else{
			gameObject.GetComponent<EnemyShooting>().enabled = true;
		}
		
	}
	
	
	void StandTimer()
	{
		timer += Time.deltaTime;
		float timeToStand = 5f;
		if(timer >= timeToStand)
		{
			aimEnemy = false;
			timer = 0;
		}
	}
    
    void Shooting ()
    {
        // Stop the enemy where it is.
        nav.Stop();
    }
    
    
    void Chasing ()
    {
        // Create a vector from the enemy to the last sighting of the player.
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
        
        // If the the last personal sighting of the player is not close...
        if(sightingDeltaPos.sqrMagnitude > 4f )
            // ... set the destination for the NavMeshAgent to the last personal sighting of the player.
            nav.destination = enemySight.personalLastSighting;
        
        // Set the appropriate speed for the NavMeshAgent.
        nav.speed = chaseSpeed;
        
        if(enemySight.personalLastSighting != playerPosition.position)
		{
			search = true;
		}
        
    }
	
	
	void Search()
	{
		if(enemySight.personalLastSighting == player.position)
		{
			search = false;
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
		
		if(nav.destination == playerPosition.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            // ... increment the timer.
            patrolTimer += Time.deltaTime;
            
            // If the timer exceeds the wait time...
            if(patrolTimer >= patrolWaitTime)
            {
                // ... increment the wayPointIndex.
                if(counter == 3){
                    // ... reset last global sighting, the last personal sighting and the timer.
					playerPosition.position = playerPosition.resetPosition;
                	enemySight.personalLastSighting = playerPosition.resetPosition;
					search = false;
					counter = 0;
				}
                else{
					search = true;
                    counter++;
				}
                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else{
            // If not near a destination, reset the timer.
            patrolTimer = 0;
		}
        // Set the destination to the patrolWayPoint.
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
                // ... increment the wayPointIndex.
                if(wayPointIndex == patrolWayPoints.Length - 1)
                    wayPointIndex = 0;
                else
                    wayPointIndex++;
                
                // Reset the timer.
                patrolTimer = 0;
            }
        }
        else
            // If not near a destination, reset the timer.
            patrolTimer = 0;
        
        // Set the destination to the patrolWayPoint.
        nav.destination = patrolWayPoints[wayPointIndex].position;
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