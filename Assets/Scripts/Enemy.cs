﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour {
	GameObject player ;
	NavMeshAgent agent; 
	public bool playerDetected;
	public Vector3 lastPlayerSighting;

	public float wanderRadius;
	public float wanderTimer;
	private float timer;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		agent = GetComponent<NavMeshAgent>();
		playerDetected = false;
		// Use this for initialization
		timer = wanderTimer;
	}
	
	// Update is called once per frame
	void Update () {

        if (!agent.enabled)
        {
            return;
        }

		// If the enemy and the player have health left...
		if (playerDetected) {
			if(LineOfSight(player.transform))
			{
				// ... set the destination of the nav mesh agent to the player.
				agent.SetDestination (player.transform.position);
				lastPlayerSighting = player.transform.position;
				//			Vector3 distance = (player.transform.position - transform.position);

				//			nav.nextPosition = (nav.speed*Time.deltaTime)*distance.normalized;
			} else {
				// ... disable the nav mesh agent.
				Vector3 distanceToLastSighting = transform.position - lastPlayerSighting;
				float distance = distanceToLastSighting.magnitude;
				if(distance >= wanderRadius){
					agent.SetDestination(lastPlayerSighting);
				} else {
					wander();
				}

			}
		} else {
			wander();
		}


	}
	void wander(){
		timer += Time.deltaTime;

		if (timer >= wanderTimer) {
			Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
			agent.SetDestination(newPos);
			timer = 0;
		}
	}


//	void OnTriggerEnter(Collider col){
//		if (col.gameObject.name == "Player") {
//			playerDetected = true;
//		}
//	}
//	void OnTriggerExit(Collider col){
//		if (col.gameObject.name == "Player") {
//			playerDetected = false;
//		}
//	}

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


	public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
		Vector3 randDirection = Random.insideUnitSphere * dist;

		randDirection += origin;

		NavMeshHit navHit;

		NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

		return navHit.position;
	}
}
