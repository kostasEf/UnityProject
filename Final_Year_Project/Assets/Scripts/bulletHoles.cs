using UnityEngine;
using System.Collections;

public class bulletHoles : MonoBehaviour {
	
	public GameObject[] bulletTex;
	public bool hasGun;
	// Update is called once per frame
	void Update () 
	{
		hasGun = GameObject.Find ("Main Camera").GetComponent<AimAndShoot>().hasGun;
		RaycastHit hit;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Vector3 position = GameObject.Find("Position").transform.position;
		
        if ( Input.GetMouseButtonDown(0) && Physics.Raycast(position, fwd,out hit, 20.0f) && hasGun == true && GameObject.Find ("Main Camera").GetComponent<AimAndShoot>().ammo != 0)
		{
			if(hit.collider.tag != "Target" && hit.collider.tag != "Ally")
			{
				Instantiate(bulletTex[Random.Range(0,3)], hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
			}
		}
	}
}
