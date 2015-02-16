using UnityEngine;
using System.Collections;

public class StrelaShooting : MonoBehaviour
{
    public float maximumDamage = 120f;                  // The maximum potential damage per shot.
    public float minimumDamage = 45f;                   // The minimum potential damage per shot.
    public AudioClip shotClip;                          // An audio clip to play when a shot happens.
    public float flashIntensity = 3f;                   // The intensity of the light when the shot happens.
    public float fadeSpeed = 10f;                       // How fast the light will fade after the shot.
    public int num;
	
    private Animator anim;                              // Reference to the animator.
    private HashIDs hash;                               // Reference to the HashIDs script.
    private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
    private Light laserShotLight;                       // Reference to the laser shot light.
    private SphereCollider col;                         // Reference to the sphere collider.
    private Transform enemy;                           // Reference to the enemy's transform.
    public bool shooting;                              // A bool to say whether or not the enemy is currently shooting.
    private float scaledDamage;                         // Amount of damage that is scaled by the distance from the enemy.
    private int p1, p2, p3, p4;
    
    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.light;
        col = GetComponent<SphereCollider>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        enemy = GameObject.FindGameObjectWithTag("Target").transform;
        // The line renderer and light are off to start.
        laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;
        
        // The scaledDamage is the difference between the maximum and the minimum damage.
        scaledDamage = maximumDamage - minimumDamage;
    }
    
    
    void Update ()
    {
		
		
        // Cache the current value of the shot curve.
        float shot = anim.GetFloat(hash.shotFloat);
        
        // If the shot curve is peaking and the enemy is not currently shooting...
        if(shot > 0.5f && !shooting)
            // ... shoot
            Shoot();
        
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
	
	void OnTriggerStay (Collider other)
    {
		// If the enemy has entered the trigger sphere...
        if(other.tag == "Target")
        {
			enemy = other.transform;
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		// If the enemy leaves the trigger zone...
		if(other.tag == "Target")
			// ... the enemy is not in sight.
			enemy = GameObject.FindGameObjectWithTag("Target").transform;
	}
    
    void OnAnimatorIK (int layerIndex)
    {
        // Cache the current value of the AimWeight curve.
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);
        
        // Set the IK position of the right hand to the enemy's centre.
        anim.SetIKPosition(AvatarIKGoal.RightHand, enemy.position + Vector3.up);
        
        // Set the weight of the IK compared to animation to that of the curve.
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }
    
    
    void Shoot ()
    {
        // The enemy is shooting.
        shooting = true;
		
        // The fractional distance from the enemy, 1 is next to the enemy, 0 is the enemy is at the extent of the sphere collider.
        float fractionalDistance = (col.radius - Vector3.Distance(transform.position, enemy.position)) / col.radius;
    
        // The damage is the scaled damage, scaled by the fractional distance, plus the minimum damage.
        float damage = scaledDamage * fractionalDistance + minimumDamage;
		
		//GameObject.FindGameObjectWithTag("Target").GetComponent<EnemyAI2>().EnemyDamage(10f);
		enemy.GetComponentInChildren<EnemyAI2>().EnemyDamage(10f);
		
        // Display the shot effects.
        ShotEffects();
    }
    
    
    void ShotEffects ()
    {
		Vector3 up = new Vector3(0f, 2f, 0f);
        // Set the initial position of the line renderer to the position of the muzzle.
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        
        // Set the end position of the enemy's centre of mass.
        laserShotLine.SetPosition(1, enemy.position + up);
        
        // Turn on the line renderer.
        laserShotLine.enabled = true;
        
        // Make the light flash.
        laserShotLight.intensity = flashIntensity;
        
        // Play the gun shot clip at the position of the muzzle flare.
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }
}