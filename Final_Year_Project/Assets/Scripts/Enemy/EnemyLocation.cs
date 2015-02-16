using UnityEngine;
using System.Collections;

public class EnemyLocation : MonoBehaviour {
	
	public GameObject[] enemies;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	
	
	// Update is called once per frame
	void Update () 
	{
		// Find an array of the siren gameobjects.
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag(Tags.enemy);
		
		// Set the enemies array to have the same number of elements as there are gameobjects.
		enemies = new GameObject[enemyGameObjects.Length];
		
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemyGameObjects[i];
        }
	}
}
