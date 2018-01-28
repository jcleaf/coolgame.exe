using UnityEngine;

public class Player : MovingObject
{
	public bool dead;
    private Rigidbody _body;
    private Animator _anim;
	private Beacon beacon;
    public float Speed = 10f;
    public float DashSpeed = 30f;
    public float DashTime = 0.2f;
    public float DashCooldown = 0.5f;

    public IntReference playerHealth;

    public PlayerState state;
    private float dashCount = 0;
    private float dashCooldown = 0;

    // Some default direction
    private Vector3 moveDir = Vector3.zero;
    private Vector3 dashDir = new Vector3(1, 0, 0);

    private float initialDrag;

	private bool _spacedeath;
	private AudioSource _spacedeathaudio;
	private AudioSource _deathaudio;
	private AudioSource _hurtaudio;
	private AudioSource _walkaudio;
	private AudioSource _dashaudio;
    public enum PlayerState
    {
        Walking, Dashing
    }

	void Start()
    {
		_spacedeath = false;
		AudioSource[] audio =  GetComponents<AudioSource> ();
		_spacedeathaudio = audio [0];
		_deathaudio = audio [1];
		_hurtaudio = audio [2];
		_walkaudio = audio [3];
		_dashaudio = audio [4];
		playerHealth.value = 100;
        _body = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
		dead = false;
        initialDrag = GetComponent<Rigidbody>().drag;
	}

    private void OnCollisionEnter(Collision collision)
	{
		Debug.Log (collision.gameObject.tag);
		if (collision.gameObject.tag == "Enemy") {
			Enemy enemy = collision.gameObject.GetComponent<Enemy> ();
			if (enemy.playerDetected) {
                playerHealth.value -= enemy.damage;
				_hurtaudio.Play ();
                if (playerHealth <= 0) {
					if (!dead) {
						_deathaudio.Play ();
					}
					dead = true;
					playerHealth.value = 0;
                }
			}
		}
        if (collision.gameObject.tag == "Beacon")
        {
			beacon = collision.gameObject.GetComponent<Beacon> ();
            beacon.Activate();
        }
    }

    public override void Update ()
    {
        base.Update();

        if (inSpace)
        {
			if (!_spacedeath) {
				_spacedeathaudio.Play ();
				_spacedeath = true;
			}
            return;
        }

        // State transitions
        switch (state) {
			case PlayerState.Walking:

				dashCooldown -= Time.deltaTime;
                if (Input.GetButtonDown("Dash") && dashCooldown < 0)
                {
					_dashaudio.Play ();
                    state = PlayerState.Dashing;
                    dashCount = DashTime; // something?
                }
                break;
			case PlayerState.Dashing:
				dashCount -= Time.deltaTime;
                if (dashCount < 0) {
                    state = PlayerState.Walking;
                    dashCooldown = DashCooldown;
                    // TODO? Update the player input so the gravity and such makes sense?
                }
                break;
        }

        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDir.sqrMagnitude > 0.4) {
            dashDir = moveDir.normalized;
        }
		if (!_walkaudio.isPlaying && moveDir.sqrMagnitude > 0.0) {
			_walkaudio.Play ();
		}
        _anim.SetFloat("MoveSpeed", moveDir.sqrMagnitude);
        _anim.SetBool("Roll", state == PlayerState.Dashing);
	}

    private void FixedUpdate()
    {
        if (inSpace)
        {
            return;
        }

        switch (state)
        {
            case PlayerState.Walking:
                _body.MovePosition(_body.position + moveDir * Speed * Time.fixedDeltaTime);
                break;
            case PlayerState.Dashing:
                _body.MovePosition(_body.position + dashDir * DashSpeed * Time.fixedDeltaTime);
                break;
        }
    }

    public override void StopBeingSucked()
    {
        base.StopBeingSucked();

        if (!inSpace)
        {
            GetComponent<Rigidbody>().drag = initialDrag;
        }
    }

    public override Vector3 GetMoveDir()
    {
        return moveDir;
    }
}
