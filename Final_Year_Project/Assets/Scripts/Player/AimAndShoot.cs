using UnityEngine;
using System.Collections;

public class AimAndShoot : MonoBehaviour {

	public int ammo = 10;
	public int totalAmmo = 20;
	private GUIText ammoGUI;
	
	private LineRenderer laserShotLine;                 // Reference to the laser shot line renderer.
    private Light laserShotLight;                       // Reference to the laser shot light.
	public float flashIntensity = 3f;                   // The intensity of the light when the shot happens.
    public float fadeSpeed = 10f;                       // How fast the light will fade after the shot.
	public AudioClip shotClip;                          // An audio clip to play when a shot happens.
	public float timer = 0f;
	
	private GameObject item;
	private GUIText guiTextCenter, guiTextBot;
	private string itemName;
	
	private Transform enemy;
	private EnemyAI2 enemyAI;
	public bool hasGun = false;
	
	
	void Awake()
	{		
		guiTextCenter = GameObject.Find("GUI_text_center").guiText;
		guiTextBot = GameObject.Find("GUI_text_bottom").guiText;
		laserShotLine = GetComponentInChildren<LineRenderer>();
        laserShotLight = laserShotLine.gameObject.light;
		laserShotLine.enabled = false;
        laserShotLight.intensity = 0f;
		ammoGUI = GameObject.Find("GUI Text_Ammo").guiText;
	}
	
	void Update() 
	{
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);		
		FadeTimer();
		
		
		
		if(Input.GetKeyDown(KeyCode.R) && totalAmmo != 0)
		{
			if(totalAmmo - (10 - ammo) < 0)
			{
				ammo = totalAmmo;
				totalAmmo = 0;
			}else{
				
				GameObject.Find("lazergun").animation.CrossFade("Reload");
				GameObject.Find("lazergun").audio.Play();
				totalAmmo = totalAmmo - (10 - ammo);
				ammo = 10;
			}
		}
		
		if(Input.GetMouseButtonDown(0) && hasGun && ammo > 0)
		{
			ammo--;
	        if (Physics.Raycast(transform.position, fwd,out hit, 200.0f))
			{
				ShotEffects(hit.point);
				
				Debug.DrawLine(transform.position, hit.point, Color.cyan);
				if(hit.collider.gameObject.tag == "Enemy")
				{
						enemy = hit.collider.transform.root;
		        		enemyAI = enemy.GetComponent<EnemyAI2>(); 
						item = hit.collider.gameObject;
						itemName = item.name;
						
						if(itemName == "char_robotGuard_Neck")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(50f);
							enemyAI.aimEnemy = true;
						}
						
						if(itemName == "char_robotGuard_Hips")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(25f);
							enemyAI.aimEnemy = true;
						}
						
						if(itemName == "char_robotGuard_LeftLeg")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(25f);
							enemyAI.aimEnemy = true;
						}
						
						if(itemName == "char_robotGuard_RightLeg")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(25f);
							enemyAI.aimEnemy = true;
						}
						
						if(itemName == "char_robotGuard_LeftArm")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(25f);
							enemyAI.aimEnemy = true;
						}
						
						if(itemName == "char_robotGuard_RightArm")
						{
							guiTextBot.text = "Shot " + itemName;
							enemyAI.EnemyDamage(25f);
							enemyAI.aimEnemy = true;
						}
				}
			}
		}
    }	
	
	void ShotEffects (Vector3 point)
    {
        // Set the initial position of the line renderer to the position of the muzzle.
        laserShotLine.SetPosition(0, laserShotLine.transform.position);
        
        // Set the end position of the player's centre of mass.
        laserShotLine.SetPosition(1, point );
        
        // Turn on the line renderer.
        laserShotLine.enabled = true;
        
        // Make the light flash.
        laserShotLight.intensity = flashIntensity;
        
        // Play the gun shot clip at the position of the muzzle flare.
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);
    }
	
	void FadeTimer()
	{
		timer += Time.deltaTime;
		float timeToStand = 0.1f;
		if(timer >= timeToStand)
		{
			laserShotLine.enabled = false;
			timer = 0;
		}
	}
	
	void OnGUI()
	{
		if(hasGun)
		{
			ammoGUI.text = ammo + "/" + totalAmmo;
		}
	}
}
