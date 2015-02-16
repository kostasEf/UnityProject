using UnityEngine;
using System.Collections;

public class DoorLogic : MonoBehaviour {
	
	public Transform theDoor;
	
	private bool drawGUI = false;
	private bool doorIsClosed = true;
	
	// Update is called once per frame
	void Update () 
	{
		if(drawGUI == true && Input.GetKeyDown(KeyCode.E))
		{
			changeDoorState();
		}
	}
	
	void OnTriggerEnter (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			drawGUI = true;
		}
	}
	
	void OnTriggerExit (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			drawGUI = false;
		}
	}
	
	void OnGUI()
	{
		if(drawGUI == true)
		{	
			if(doorIsClosed == true)
			{
				GUI.Label(new Rect(Screen.width*0.5f - 31, Screen.height*0.5f - 20, 210, 22), "Press E to Open");
			}else{
				GUI.Label(new Rect(Screen.width*0.5f - 31, Screen.height*0.5f - 20, 210, 22), "Press E to Close");
			}
		}
	}
	
	void changeDoorState()
	{
		if(doorIsClosed == true)
		{
			theDoor.animation.CrossFade("Open");
			doorIsClosed = false;
		}else if(doorIsClosed == false)
		{
			theDoor.animation.CrossFade("Close");
			doorIsClosed = true;
		}
	}
}
