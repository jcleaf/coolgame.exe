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
		playerDetected = true;
	}
	
	// Update is called once per frame
	void Update () {
		// If the enemy and the player have health left...
		if(LineOfSight(player.transform) && playerDetected)
		{
			// ... set the destination of the nav mesh agent to the player.
			nav.SetDestination (player.transform.position);
//			Vector3 distance = (player.transform.position - transform.position);

//			nav.nextPosition = (nav.speed*Time.deltaTime)*distance.normalized;
		}
		// Otherwise...
		else
		{
			// ... disable the nav mesh agent.
			if (self != null)
				
				nav.velocity = Vector3.zero;
				nav.SetDestination (transform.position);
		}
	}
	/*
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
	*/
	double fov = 360.0;
	private RaycastHit hit;

	bool LineOfSight(Transform target) {
		if (Vector3.Angle(target.position - transform.position, transform.forward) <= fov &&
			Physics.Linecast(transform.position, target.position, out hit) &&
			hit.collider.transform == target) {
			return true;
		}
		return false;
	}
}
