using UnityEngine;
using System.Collections;

public class JustDie : MonoBehaviour {
	
	private Animator anim;                     		        // Reference to the animator component.
	private HashIDs hash; 
	
	
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
	}
	
	void Update()
	{
		anim.SetBool(hash.deadBool, true);
	}
	
}
