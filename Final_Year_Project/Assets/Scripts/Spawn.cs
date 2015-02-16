using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	
	public GameObject enemy;
	private bool spawn = true;
	
	// Use this for initialization
	
	
	void OnTriggerEnter( Collider other)
	{
		if(other.tag == "Player" && spawn)
		{
			Vector3 position1 = new Vector3(138f, 8f, 49f);
			Vector3 position2 = new Vector3(134f, 8f, 41f);
			Vector3 position3 = new Vector3(134f, 8f, 33);
			Instantiate(enemy, position1, Quaternion.identity);
			Instantiate(enemy, position2, Quaternion.identity);
			Instantiate(enemy, position3, Quaternion.identity);
			spawn = false;
		}
		
	}
	
}
