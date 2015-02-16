using UnityEngine;
using System.Collections;

public class DoorUnlock : MonoBehaviour 
{
	public Texture unlockedTex;
	public Transform theLeftSlideDoor;
	public Transform theRightSlideDoor;
	
	private GameObject player;
	private PlayerInventory playerInventory;		// Reference to the player's inventory.
	private bool drawGUI = false;
	private bool drawHint = false;
	private bool doorIsClosed = true;
	public bool hasKeys = false;

	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();
	}
	
	void Update () 
	{
		if(playerInventory.prop_keycard_001 == true && playerInventory.prop_keycard_002 == true)
		{
			hasKeys = true;
			drawHint = false;
		}
		
		if(drawGUI == true && Input.GetKeyDown(KeyCode.E) && hasKeys == true)
		{
			changeDoorState();
			DoorUnlocking();
		}
	}
	
	
	void OnTriggerEnter (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			drawGUI = true;
			
			if(hasKeys == false)
			{
				drawHint = true;
			}
			
			
		}
	}
	
	void OnTriggerExit (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			drawGUI = false;
			drawHint = false;
		}
	}
	
	void OnGUI()
	{
		if(drawGUI == true && drawHint == false)
		{	
			if(doorIsClosed == true)
			{
				GUI.Label(new Rect(Screen.width*0.5f - 31, Screen.height*0.5f - 20, 210, 22), "Press E to Open");
			}
		}
		if(drawHint == true)
		{
			GUI.Label(new Rect(Screen.width*0.5f - 31, Screen.height*0.5f - 50, 210, 22), "Two Keycards are needed");
		}
	}
	
	void changeDoorState()
	{
		if(doorIsClosed == true)
		{
			theLeftSlideDoor.animation.CrossFade("SlideOpen");
			theRightSlideDoor.animation.CrossFade("SlideOpen2");
			doorIsClosed = false;
		}
	}
	
	void DoorUnlocking()
	{
		Renderer screen = transform.Find("prop_switchUnit_screen").renderer;
		screen.material.mainTexture = unlockedTex;
		audio.Play();
	}
	
}
