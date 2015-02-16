using UnityEngine;
using System.Collections;

public class Cell_Door : MonoBehaviour {

	private float timer = 0f;
	
	
	// Update is called once per frame
	void Awake () 
	{
		
		gameObject.animation.CrossFade("Cell_Open");
			
		
	}
	
	
	
}
