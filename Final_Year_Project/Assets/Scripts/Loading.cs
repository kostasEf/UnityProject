using UnityEngine;
using System.Collections;
using System;

public class Loading : MonoBehaviour
{
    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	
    
    void Awake ()
    {
        nav = GetComponent<NavMeshAgent>();
		
    }
    
    
    void Update ()
    {
		
        RunForestRun();
		
		// Create a vector from the enemy to the last sighting of the player.
        Vector3 sightingDeltaPos = gameObject.transform.position - GameObject.Find("Goal").transform.position;
        
        // If the the last personal sighting of the player is not close...
        if(sightingDeltaPos.sqrMagnitude < 30f )
		{
			Application.LoadLevel(1);
		}
		
    }
    
    
    void RunForestRun ()
    {
		nav.speed = 5f;
		nav.destination = GameObject.Find("Goal").transform.position;
		
    }
	
	
	
}