using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
	public float health;
	public Texture2D texture1;
	public Material mat;
	public float w;
	public float h;
	
	
	private MovementC movement;
	private PlayerPosition playerPosition;
	private SceneFadeInOut sceneFadeInOut;
	private float timer;
	public bool playerDead;
	private GUIText healthGUI;
	
	void Awake()
	{
		health = 1000f;
		movement = GetComponent<MovementC>();
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();
		playerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<PlayerPosition>();
		healthGUI = GameObject.Find("GUI Text_Health").guiText;
	}
	
	void Update()
	{
		if(health <= 0f)
		{
			playerDead = true;
			movement.runSpeed = 0f;
			movement.crchSpeed = 0f;
			movement.walkSpeed = 0f;
			playerPosition.position = playerPosition.resetPosition;
		}
		
		float healthy = 1f-(health/100);
		if(healthy <= 0)
		{
			healthy = 0.1f;
		}
		mat.SetFloat("_Cutoff", healthy);
		int h = (int)health;
		healthGUI.text = h.ToString();

	}
	
	
	
	public void TakeDamage(float amount)
	{
		health -= amount;
	}
}
