using UnityEngine;
using System.Collections;

public class StrelaSight : MonoBehaviour 
{
	public float fieldOfViewAngle = 180f;
	public bool enemyInSight;
	public Vector3 personalLastSighting;
	public Vector3 headPosition;
	
	private NavMeshAgent nav;
	private SphereCollider col;
	private Animator anim;								// Reference to the Animator.
	private HashIDs hash;							    // Reference to the HashIDs.
	public GameObject enemy;
	private Vector3 previousSighting;
	public float velocity;

	
	
	
	void Awake()
	{
		nav = GetComponent<NavMeshAgent>();
		col = GetComponent<SphereCollider>();
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		enemy = GameObject.FindGameObjectWithTag("Target");
	}
	
	void Update ()
	{
		
		// If the enemy is alive...
		if(enemy.GetComponent<EnemyAI2>().health > 0f)
		{
			// ... set the animator parameter to whether the enemy is in sight or not.
			anim.SetBool(hash.playerInSightBool, enemyInSight);
		}else{
			// ... set the animator parameter to false.
			anim.SetBool(hash.playerInSightBool, false);
			enemyInSight = false;
		}
	}
	

	void OnTriggerStay (Collider other)
    {
		// If the enemy has entered the trigger sphere...
        if(other.tag == "Target")
        {
			enemy = other.gameObject;
			// By default the enemy is not in sight.
			enemyInSight = false;
			
			Vector3 poz = new Vector3(0f, 1.65f, 0f);
			headPosition = transform.position + poz;
			//Vector3 headPosition = GameObject.Find("char_robotGuard_Head").transform.position;
			
			// Create a vector from the enemy to the enemy and store the angle between it and forward.
            Vector3 direction = other.transform.position - headPosition;
			float angle = Vector3.Angle(direction, transform.forward);
			
			// If the angle between forward and where the enemy is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				
				RaycastHit hit;
				Vector3 positionY = new Vector3(0f, 2f, 0f);
				
				// ... and if a raycast towards the player hits something...
				if(Physics.Raycast(headPosition, direction + positionY, out hit, col.radius))
				{
				
					//Debug.DrawLine (headPosition, hit.point, Color.cyan);
					// ... and if the raycast hits the player...
					if(hit.collider.tag == "Enemy")
					{
						// ... the player is in sight.
						enemyInSight = true;
		
					}
				}
			}
			
			
			
        }
    }
	
	
	void OnTriggerExit (Collider other)
	{
		// If the enemy leaves the trigger zone...
		if(other.tag == "Target")
			// ... the enemy is not in sight.
			enemyInSight = false;
			enemy = GameObject.FindGameObjectWithTag("Target");
	}
	
	
}
