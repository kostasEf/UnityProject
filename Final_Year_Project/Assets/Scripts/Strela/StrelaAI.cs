using UnityEngine;
using System.Collections;

public class StrelaAI : MonoBehaviour
{
    public float walkSpeed = 5f;                          // The nav mesh agent's speed when patrolling.
	public float runSpeed = 8f;                          // The nav mesh agent's speed when patrolling.
    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
    private Transform player;                               // Reference to the player's transform.
	private StrelaSight strelaSight;
    
    void Awake ()
    {
        // Setting up the references.
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		strelaSight = GameObject.FindGameObjectWithTag("Ally").GetComponent<StrelaSight>();
    }
    
    
    void Update ()
    {
		if(Vector3.Distance(transform.position, GameObject.Find("strelaCover").transform.position) > 0f)
		{
			nav.SetDestination(GameObject.Find("strelaCover").transform.position);
		}
		if(gameObject.GetComponent<StrelaShooting>().shooting == true){
            // ... shoot.
            nav.Stop();
		}
		else
		{
			if(GameObject.Find("Main Camera").GetComponent<AimAndShoot>().totalAmmo == 0)
			{
				Resupply();
			}else{
				Follow();
			}
		}
    	
    }
	
	
	void Resupply()
	{
		Vector3 sightingPos = player.position - transform.position;
        
        // If the player is more that 4 units away...
        if(sightingPos.sqrMagnitude > 3f)
		{
            // ... set strela to come close to the player player.
            nav.destination = player.position;
			nav.speed = runSpeed;
			
		}else if(sightingPos.sqrMagnitude < 3f)
		{	
			nav.speed = walkSpeed;
			nav.Stop();
			GameObject.Find("Main Camera").GetComponent<AimAndShoot>().totalAmmo = 40;
		}
		
	}
    
    void Follow ()
    {
		
        Vector3 sightingPos = player.position - transform.position;
        if(sightingPos.sqrMagnitude > 10f)
		{
            nav.destination = player.position;
			nav.speed = runSpeed;
		}else if(sightingPos.sqrMagnitude < 10f)
		{	
			nav.speed = walkSpeed;
			nav.Stop();
		}
    }

}