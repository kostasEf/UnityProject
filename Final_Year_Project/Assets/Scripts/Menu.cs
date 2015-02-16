using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	
	private MouseLook look1;
	private MouseLook look2;
	private CharacterMotor chMotor;
	private MovementC movement;
	private SceneFadeInOut sceneFadeInOut;
	private bool playerDead = false;
	private bool pause = false;
	
	// Use this for initialization
	void Start () 
	{
		Screen.lockCursor =true;
		look1 = gameObject.GetComponent<MouseLook>();
		look2 = GameObject.Find("Main Camera").GetComponent<MouseLook>();
		chMotor = gameObject.GetComponent<CharacterMotor>();
		movement = gameObject.GetComponent<MovementC>();
		sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		playerDead = gameObject.GetComponent<PlayerHealth>().playerDead;
		
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			pause = !pause;
			look1.enabled = !look1.enabled;
			look2.enabled = !look2.enabled;
			if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
				Screen.lockCursor = true;
				Screen.showCursor = false;
            } else {
                Time.timeScale = 0;
				Screen.lockCursor =false;
				Screen.showCursor = true;
				
            }
		}
	}
	
	void OnGUI()
	{
		if(playerDead == true || pause)
		{
			look1.enabled = false;
			look2.enabled = false;
			Screen.lockCursor =false;
			
			if(GUI.Button(new Rect(Screen.width*0.5f-50, 200-20, 100, 40), "Restart"))
			{
				Application.LoadLevel(1);
			}
			
			if(GUI.Button(new Rect(Screen.width*0.5f-50, 240, 100, 40), "Exit Game"))
			{
				Application.LoadLevel(0);
				Time.timeScale = 1;
			}
		}
	}
}
