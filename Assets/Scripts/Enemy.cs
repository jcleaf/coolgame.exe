﻿using UnityEngine;
using UnityEngine.AI;
public class Enemy : MovingObject
{
	GameObject player;
	NavMeshAgent agent;
    Animator anim;

	public bool playerDetected;
	public Vector3 lastPlayerSighting;

	public float wanderRadius;
	public float wanderTimer;
	private float timer;

	private float btimer;
	public float bumpedTimer;
	private bool bumped;

    private Rigidbody rb;
    private float initialDrag;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponentInChildren<Animator>();
		agent = GetComponent<NavMeshAgent>();
		playerDetected = false;
		// Use this for initialization
		timer = wanderTimer;
		bumped = false;
		btimer = bumpedTimer;
        rb = GetComponent<Rigidbody>();
        initialDrag = rb.drag;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			bumped = true;
		}
	}

	// Update is called once per frame
	void Update () {

        if (inSpace)
        {
            return;
        }

        // If the enemy and the player have health left...
        if (playerDetected && agent.enabled) {
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
			//If got bumped
			if (bumped) {
				agent.enabled = false;
                rb.drag = 0f;
				btimer += Time.deltaTime;
				if (btimer >= bumpedTimer) {
                    //go back to normal, remove bumped state. wander!
                    rb.drag = initialDrag;
					bumped = false;
					agent.enabled = true;
			      	btimer = 0;
					wander ();
				}
			} else {
				wander();
			}

		}

        anim.SetFloat("MoveSpeed", agent.velocity.sqrMagnitude);

	}
	void wander(){
		timer += Time.deltaTime;

		if (timer >= wanderTimer && agent.enabled) {
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

    public override void BeingSucked(Vector3 suckForce)
    {
        base.BeingSucked(suckForce);

        agent.enabled = false;
    }
}
