using UnityEngine;
using System.Collections;

public class Throw : MonoBehaviour {
		
	public bool wasThrown = false;
	public bool pickedUp;
	public bool hasLanded = false;
	public AudioClip mugClip;
	
	
	void Awake()
	{
		pickedUp = true;
	}
	
	void Update()
	{
		if(pickedUp == true)
		{
			transform.position = GameObject.Find("Throw_Position").transform.position;
			transform.rotation = GameObject.Find("Throw_Position").transform.rotation;
		}
		
		if(Input.GetMouseButtonDown(0) && pickedUp == true)
		{
			rigidbody.isKinematic = false;
			rigidbody.AddForce(transform.forward * 50);
			rigidbody.AddForce(transform.up * 10);
			rigidbody.useGravity = true;
			transform.parent = null;
		}
		
		if(rigidbody.velocity.magnitude > 0)
		{
			pickedUp = false;
			wasThrown = true;
		}
			
		
		if(rigidbody.velocity.magnitude == 0 && wasThrown == true)
		{
			wasThrown = false;
			hasLanded = true;
			AudioSource.PlayClipAtPoint(mugClip, transform.position);
			Destroy(gameObject, 5f);
		}
			
	}
	
	
}
