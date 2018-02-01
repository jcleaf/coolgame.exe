using UnityEngine;
using UnityEngine.AI;
public class Enemy : MovingObject
{
	GameObject player;
	NavMeshAgent agent;
    Animator anim;
    [SerializeField]
    Reaction reaction;

	public bool playerDetected;
	public Vector3 lastPlayerSighting;

	public float wanderRadius;
	public float wanderTimer;
	private float timer;

	private float btimer;
	public float bumpedTimer;
	private bool bumped;
	private bool sighted;
    public int damage;
	private AudioSource _playerDetectedAudio;

    public float pushedStrength;
    private bool dashed;
    private Vector3 playerSpot;

    public float pushOffWallForce;
    private float initialDrag;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _playerDetectedAudio = GetComponent<AudioSource>();
    }

	void Start ()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        
		playerDetected = false;
		timer = wanderTimer;
		bumped = false;
		btimer = bumpedTimer;
        initialDrag = rb.drag;
		sighted = false;
	}

	void OnCollisionEnter(Collision col)
    {
        string layerName = LayerMask.LayerToName(col.gameObject.layer);
        switch (layerName)
        {
            case "Player":
                playerSpot = col.transform.position;
                bumped = true;
                dashed = col.gameObject.GetComponent<Player>().state == Player.PlayerState.Dashing;
                break;
            case "Walls":
                if (!agent.enabled)
                {
                    Vector3 dir = col.contacts[0].normal;
                    rb.AddForce(dir * pushOffWallForce, ForceMode.Impulse);
                }
                break;
        }
	}

	// Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (inSpace)
        {
            return;
        }

        // If the enemy and the player have health left...
        if (playerDetected && agent.enabled) {
            bumped = false;
            btimer = 0f;

            if (LineOfSight(player.transform))
			{
				if (!sighted) {
					_playerDetectedAudio.Play ();
					sighted = true;
				} 

				// ... set the destination of the nav mesh agent to the player.
				agent.SetDestination (player.transform.position);
				lastPlayerSighting = player.transform.position;
				//			Vector3 distance = (player.transform.position - transform.position);

				//			nav.nextPosition = (nav.speed*Time.deltaTime)*distance.normalized;
                reaction.Exclaim();
			} else {
				
				// ... disable the nav mesh agent.
				Vector3 distanceToLastSighting = transform.position - lastPlayerSighting;
				float distance = distanceToLastSighting.magnitude;
				if(distance >= wanderRadius){
					agent.SetDestination(lastPlayerSighting);
				} else {
                    reaction.Question();
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

        if (dashed)
        {
            bumped = true;
            dashed = false;
            agent.enabled = false;
            rb.drag = 0f;

            Vector3 pushDir = transform.position - playerSpot;
            pushDir.y = 0f;
            pushDir.Normalize();
            rb.AddForce(pushDir * pushedStrength, ForceMode.Impulse);
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

    public override Vector3 GetMoveDir() {
        return agent.velocity;
    }
}
