using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    public float maximumDamage;                   		// The maximum potential damage per shot.
    public float minimumDamage;                   		// The minimum potential damage per shot.
    public AudioClip shotClip;                          // An audio clip to play when a shot happens.
    public float flashIntensity = 3f;                   // The intensity of the light when the shot happens.
    public float fadeSpeed = 10f;                       // How fast the light will fade after the shot.
    public int num;
	private string difficulty;
	public Vector3 headPosition;
	
    private Animator animator;                              // Reference to the Animator.
    private HashIDs hash;                               // Reference to the HashIDs script.
    private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
    private Light laserShotLight;                       // Reference to the laser shot light.
    private SphereCollider sphereCollider;   // Reference to the sphere sphereColliderlider.
    private Transform player;                           // Reference to the player's transform.
    private PlayerHealth playerHealth;                  // Reference to the player's health.
    public bool shooting;                               // A bool to say whether or not the enemy is currently shooting.
    private float scaledDamage;                         // Amount of damage that is scaled by the distance from the player.
    private int p1, p2, p3, p4;
	
    
    void Awake ()
    {
		difficulty = PlayerPrefs.GetString("difficulty");
		if(difficulty == "Hard")
		{
			maximumDamage = 100f;                  
    		minimumDamage = 75f;   
		}
		else if(difficulty == "Medium")
		{
			maximumDamage = 75f;                  
    		minimumDamage = 50f; 
		}
		else if(difficulty == "Easy")
		{
			maximumDamage = 50f;                  
    		minimumDamage = 25f; 
		}
		else
		{
			maximumDamage = 75f;                  
    		minimumDamage = 50f; 
		}
				
		animator = GetComponent<Animator>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.light;
        sphereCollider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        
        // The line renderer and light are off to start.
        laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;
        
        // The scaledDamage is the difference between the maximum and the minimum damage.
        scaledDamage = maximumDamage - minimumDamage;
    }
    
    
    void Update ()
    {
		
		num = Random.Range(0,Counter());
        // Cache the current value of the shot curve.
        float shot = animator.GetFloat(hash.shotFloat);
        
        // If the shot curve is peaking and the enemy is not currently shooting...
        if(shot > 0.5f && !shooting)
            // ... shoot
            Shoot(Counter());
        
        // If the shot curve is no longer peaking...
        if(shot < 0.5f)
        {
            // ... the enemy is no longer shooting and disable the line renderer.
            shooting = false;
            laserShotLine.enabled = false;
        }
        
        // Fade the light out.
        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
    }
	
	
    
	private int Counter()
	{
		if(GetComponent<EnemySight>().p1)
		{
			p1 = 0;
		}else{
			p1 = 1;
		}
		
		if(GetComponent<EnemySight>().p2)
		{
			p2 = 0;
		}else{
			p2 = 1;
		}
		
		if(GetComponent<EnemySight>().p3)
		{
			p3 = 0;
		}else{
			p3 = 1;
		}
		
		if(GetComponent<EnemySight>().p4)
		{
			p4 = 0;
		}else{
			p4 = 1;
		}
		
		int count = p1 + p2 + p3 + p4;
		return count;
	}
    
    void OnAnimatorIK (int layerIndex)
    {
        // Cache the current value of the AimWeight curve.
        float aimWeight = animator.GetFloat(hash.aimWeightFloat);
        
        // Set the IK position of the right hand to the player's centre.
        animator.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up);
        
        // Set the weight of the IK compared to animatoration to that of the curve.
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }
    
    
    void Shoot (int p)
    {
        // The enemy is shooting.
        shooting = true;
		
        // This should be a number that equals 1 when the distance from the enemy to 
		//the player is 0 and when the player is at the edge of the sphereColliderlider, the maximum range, the number should equal zero.
        float fractDistance = (sphereCollider.radius - Vector3.Distance(transform.position, player.position)) / sphereCollider.radius;
    
        // The damage is the scaled damage, scaled by the fractional distance, plus the minimum damage.
        float damage = scaledDamage * fractDistance + minimumDamage;
		
		int number = Random.Range(0,p);
		if(p == 0)
		{
			// The player takes damage.
        	playerHealth.TakeDamage(damage);
		}else
		{
			if(number == 0)
			{
				playerHealth.TakeDamage(damage);
			}
		}
        
        
        // Display the shot effects.
        ShotEffects();
    }
    
    
    void ShotEffects ()
    {
        // Set the initial position of the line renderer to the position of the muzzle.
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        
        // Set the end position of the player's centre of mass.
        laserShotLine.SetPosition(1, player.position );
        
        // Turn on the line renderer.
        laserShotLine.enabled = true;
        
        // Make the light flash.
        laserShotLight.intensity = flashIntensity;
        
        // Play the gun shot clip at the position of the muzzle flare.
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }
}