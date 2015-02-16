using UnityEngine;
using System.Collections;

public class DoubleDoorLogic : MonoBehaviour {
	
	
	public Transform theLeftSlideDoor;
	public Transform theRightSlideDoor;
	
	
	void OnTriggerEnter (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theLeftSlideDoor.animation.CrossFade("SlideOpen");
			theRightSlideDoor.animation.CrossFade("SlideOpen2");
		}
	}
	
	void OnTriggerExit (Collider theCollider)
	{
		if(theCollider.tag == "Player")
		{
			theLeftSlideDoor.animation.CrossFade("SlideClose");
			theRightSlideDoor.animation.CrossFade("SlideClose2");
		}
	}
	

	
	
}
