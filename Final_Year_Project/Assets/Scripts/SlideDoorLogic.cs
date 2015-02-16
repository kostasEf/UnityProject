using UnityEngine;
using System.Collections;

public class SlideDoorLogic : MonoBehaviour {
	
	
	public Transform theSlideDoor;
	private bool doorIsClosed = true;
	
	
	void OnTriggerEnter (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theSlideDoor.animation.CrossFade("SlideOpen");
		}
	}
	
	void OnTriggerExit (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theSlideDoor.animation.CrossFade("SlideClose");
		}
	}
	

	
	
}
