using UnityEngine;
using System.Collections;
 
public class FlashLight : MonoBehaviour{
 
	public bool hasFlashlight;
    private bool on = false;
	private string up = "itemUp";
 	private string down = "itemDown";
	private string upDown;
	
	void Update()
	{
		if(hasFlashlight)
		{
			OnOff();
		}
	}

	
	void OnOff()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			on = !on;
			if(on)upDown = up;
			if(!on)upDown = down;
				
			GameObject.Find("torch").animation.CrossFade(upDown);
			
		}
		if(on)
		{
			light.enabled = true;
		}else if(!on)
		{
			light.enabled = false;
		}
	}
	
}
