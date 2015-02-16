using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour 
{
	public float fieldOfViewAngle = 110f;
	public bool playerInSight, p1=false, p2=false, p3=false, p4=false;
	public Vector3 personalLastSighting;
	public Vector3 headPosition;
	public Vector3 gunPosition;
	public bool canShoot = false;
	private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
	
	private NavMeshAgent nav;
	private SphereCollider sphereCollider;
	private PlayerPosition playerPosition;
	private Animator animator;								// Reference to the animatorator.
	private HashIDs hash;							// Reference to the HashIDs.
	private GameObject player;
	private PlayerHealth playerHealth;
	private Vector3 previousSighting;
	public float velocity;

	
	
	
	void Awake()
	{
		laserShotLine = GetComponentInChildren<LineRenderer>();
		nav = GetComponent<NavMeshAgent>();
		sphereCollider = GetComponent<SphereCollider>();
		animator = GetComponent<Animator>();
		playerPosition = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerPosition>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerHealth = player.GetComponent<PlayerHealth>();
		hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
		
		
		personalLastSighting = playerPosition.resetPosition;
		previousSighting = playerPosition.resetPosition;
	}
	
	void Update ()
	{
		//CanShoot();
		velocity = player.GetComponent<MovementC>().velocity;
		// If the last global sighting of the player has changed...
		if(playerPosition.position != previousSighting)
			// ... then update the personal sighting to be the same as the global sighting.
			personalLastSighting = playerPosition.position;
		
		// Set the previous sighting to be the sighting from this frame.
		previousSighting = playerPosition.position;
		
		// If the player is alive...
		if(playerHealth.health > 0f)
			// ... set the animatorator parameter to whether the player is in sight or not.
			animator.SetBool(hash.playerInSightBool, playerInSight);
		else
			// ... set the animatorator parameter to false.
			animator.SetBool(hash.playerInSightBool, false);
	}
	

	void OnTriggerStay (Collider collider)
    {
		
		// If the player has entered the trigger sphere...
        if(collider.tag == "Player")
        {
			// By default the player is not in sight.
			playerInSight = false;
			
			Vector3 poz = new Vector3(0f, 2f, 0f);
			headPosition = transform.position + poz;
			//Vector3 headPosition = GameObject.Find("char_robotGuard_Head").transform.position;
			
			// Create a vector from the enemy to the player and store the angle between it and forward.
            Vector3 direction = collider.transform.position - headPosition;
			float angle = Vector3.Angle(direction, transform.forward);
			
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				Vector3 positionY = new Vector3(0f, 1f, 0f);
				Vector3 positionX = new Vector3(0.35f, 0f, 0f);
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(headPosition, direction - positionY, out hit, sphereCollider.radius))
				{
				
					Debug.DrawLine (headPosition, hit.point, Color.cyan);
					// ... and if the raycast hits the player...
					if(hit.collider.tag == "Player")
					{
						
						
							playerInSight = true;
							playerPosition.position = player.transform.position;
							
						
						
						p1 = true;
					}else{
						p1 = false;
					}
				}
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(headPosition, direction + positionY, out hit, sphereCollider.radius))
				{
				
					Debug.DrawLine (headPosition, hit.point, Color.cyan);
					// ... and if the raycast hits the player...
					if(hit.collider.tag == "Player")
					{
						
						
							playerPosition.position = player.transform.position;
							playerInSight = true;
						
						p2 = true;
					}else{
						p2 = false;
					}
				}
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(headPosition, direction + positionX, out hit, sphereCollider.radius))
				{
				
					Debug.DrawLine (headPosition, hit.point, Color.cyan);
					// ... and if the raycast hits the player...
					if(hit.collider.tag == "Player")
					{
						
						
							playerPosition.position = player.transform.position;
							playerInSight = true;
						
						p3 = true;
					}else{
						p3 = false;
					}
				}
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(headPosition, direction - positionX, out hit, sphereCollider.radius))
				{
				
					Debug.DrawLine (headPosition, hit.point, Color.cyan);
					// ... and if the raycast hits the player...
					if(hit.collider.tag == "Player")
					{
						
						playerPosition.position = player.transform.position;
						playerInSight = true;
	
						
						p4 = true;
					}else{
						p4 = false;
					}
				}
			}
			
			
			if(velocity > 2.5f)
			{
				// ... and if the player is within hearing range...
				if(CalculatePathLength(player.transform.position) <= sphereCollider.radius)
				{// ... set the last personal sighting of the player to the player's current position.
					personalLastSighting = player.transform.position;
				}
			}
        }
		
		
		if(collider.tag == "Throw")
		{
			if(collider.GetComponent<Throw>().hasLanded == true && personalLastSighting == playerPosition.resetPosition)
			{
				personalLastSighting = collider.transform.position;
			}
		}
    }
	
//	void CanShoot()
//	{
//		
//			// By default the player is not in sight.
//			canShoot = false;
//			
//			Vector3 poz = new Vector3(0f, 1.5f, 0f);
//			gunPosition = transform.position + poz;
//			
//			// Create a vector from the enemy to the player and store the angle between it and forward.
//            Vector3 direction = player.transform.position - gunPosition;
//			
//			
//				RaycastHit hit;
//				Vector3 positionY = new Vector3(0f, 1f, 0f);
//				Vector3 positionX = new Vector3(0.35f, 0f, 0f);
//				
//				// ... and if a raycast towards the player hits something...
//				if(Physics.Raycast(gunPosition, direction, out hit, sphereCollider.radius))
//				{
//				
//					Debug.DrawLine (gunPosition, hit.point, Color.cyan);
//					// ... and if the raycast hits the player...
//					if(hit.collider.tag == "Player")
//					{
//						// ... the player is in sight.
//						canShoot = true;
//						
//					}
//				}
//				
//				// ... and if a raycast towards the player hits something...
//				if(Physics.Raycast(gunPosition, direction + positionY, out hit, sphereCollider.radius))
//				{
//				
//					Debug.DrawLine (gunPosition, hit.point, Color.cyan);
//					// ... and if the raycast hits the player...
//					if(hit.collider.tag == "Player")
//					{
//						// ... the player is in sight.
//						canShoot = true;
//						
//					}
//				}
//				
//				// ... and if a raycast towards the player hits something...
//				if(Physics.Raycast(gunPosition, direction + positionX, out hit, sphereCollider.radius))
//				{
//				
//					Debug.DrawLine (gunPosition, hit.point, Color.cyan);
//					// ... and if the raycast hits the player...
//					if(hit.collider.tag == "Player")
//					{
//						// ... the player is in sight.
//						canShoot = true;
//						
//					}
//				}
//				
//				// ... and if a raycast towards the player hits something...
//				if(Physics.Raycast(gunPosition, direction - positionX, out hit, sphereCollider.radius))
//				{
//				
//					Debug.DrawLine (gunPosition, hit.point, Color.cyan);
//					// ... and if the raycast hits the player...
//					if(hit.collider.tag == "Player")
//					{
//						// ... the player is in sight.
//						canShoot = true;
//						
//					}
//				}
//	}
	
	
	void OnTriggerExit (Collider collider)
	{
		// If the player leaves the trigger zone...
		if(collider.gameObject == player)
			// ... the player is not in sight.
			playerInSight = false;
	}
	
	
	float CalculatePathLength (Vector3 targetPosition)
	{
		// Create a path and set it based on a target position.
		NavMeshPath path = new NavMeshPath();
		if(nav.enabled)
			nav.CalculatePath(targetPosition, path);
		
		// Create an array of points which is the length of the number of corners in the path + 2.
		Vector3 [] allPositions = new Vector3[path.corners.Length + 2];
		
		// The first point is the enemy's position.
		allPositions[0] = transform.position;
		
		// The last point is the target position.
		allPositions[allPositions.Length - 1] = targetPosition;
		
		// The points inbetween are the corners of the path.
		for(int i = 0; i < path.corners.Length; i++)
		{
			allPositions[i + 1] = path.corners[i];
		}
		
		// Create a float to store the path length that is by default 0.
		float pathLength = 0;
		
		// Increment the path length by an amount equal to the distance between each waypoint and the next.
		for(int i = 0; i < allPositions.Length - 1; i++)
		{
			pathLength += Vector3.Distance(allPositions[i], allPositions[i + 1]);
		}
		
		return pathLength;
	}
}
