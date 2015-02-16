using UnityEngine;
using System.Collections;

public class SlideDoorLogic2 : MonoBehaviour {
	
	
	public Transform theSlideDoor;
	
	private bool doorIsClosed = true;
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerEnter (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theSlideDoor.animation.CrossFade("SlideOpen2");
		}
	}
	
	void OnTriggerExit (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theSlideDoor.animation.CrossFade("SlideClose2");
		}
	}
	

	
	
}
