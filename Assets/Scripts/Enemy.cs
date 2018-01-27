using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour {
	GameObject player ;
	NavMeshAgent nav; 
	bool playerDetected;
	GameObject self;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		self = GetComponent<GameObject>();
		nav = GetComponent<NavMeshAgent>();
		playerDetected = false;
	}
	
	// Update is called once per frame
	void Update () {
		// If the enemy and the player have health left...
		if(playerDetected)
		{
			// ... set the destination of the nav mesh agent to the player.
			nav.SetDestination (player.transform.position);
		}
		// Otherwise...
		else
		{
			// ... disable the nav mesh agent.
			if(self != null)
				nav.SetDestination (self.transform.position);
		}
	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.name == "Player") {
			playerDetected = true;
		}
	}
	void OnTriggerExit(Collider col){
		if (col.gameObject.name == "Player") {
			playerDetected = false;
		}
	}
}
