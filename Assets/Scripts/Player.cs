﻿using System.Collections;
using UnityEngine;

public class Player : MovingObject
{
	
	public bool dead;
    private Animator _anim;
	private Beacon beacon;
    private InstructionHandler instructionHandler;

    public float Speed = 10f;
    public float DashSpeed = 30f;
    public float DashTime = 0.2f;
    public float DashCooldownTime = 0.5f;
    public float DashInvincibleTime = 0.25f;
    public float HurtCooldownTime = 1f;
    public float WalkSfxTime = 0.2f;
    public float WalkSfxCount = 0;

    public IntReference playerHealth;
    public BoolReference playerWonRef;
    public BoolReference beaconsLit;

    public Color damageColor;
    public Color invincibleColor;

    public PlayerState state;
    private float dashCount = 0;
    private float dashCooldownCount = 0;
    private float hurtCooldownCount = 0;

    // Some default direction
    private Vector3 moveDir = Vector3.zero;
    private Vector3 dashDir = new Vector3(1, 0, 0);

    // Last place where an instruction was generated
    private Vector3 lastInstructionPos = Vector3.zero;

    private float initialDrag;

	private bool _spacedeath;
	private AudioSource _spacedeathaudio;
	private AudioSource _deathaudio;
	private AudioSource _hurtaudio;
	private AudioSource _walkaudio;
	private AudioSource _dashaudio;
	private AudioSource _winaudio;
	private MusicLoop _musicloop;

    private SpriteRenderer spriteRenderer;

	private Camera cam;
    public enum PlayerState
    {
        Walking, Dashing
    }

    protected override void Awake()
    {
        base.Awake();
        cam = UnityEngine.Camera.main.GetComponent<Camera> ();
		GameObject music = GameObject.Find("Audio Source");

		_musicloop = music.GetComponent<MusicLoop> ();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponentInChildren<Animator>();

        AudioSource[] audio = GetComponents<AudioSource>();
        _spacedeathaudio = audio[0];
        _deathaudio = audio[1];
        _hurtaudio = audio[2];
        _walkaudio = audio[3];
        _dashaudio = audio[4];
        _winaudio = audio[5];

        playerHealth.value = 100;
        playerWonRef.value = false;
    }

	void Start()
    {
		_spacedeath = false;
        dead = false;
        initialDrag = rb.drag;
        lastInstructionPos = transform.position;

        instructionHandler = GameObject.Find("InstructionHandler").GetComponent<InstructionHandler>();
        instructionHandler.AddNextPlayerInstruction(transform.position);
	}

    private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Beacon")
        {
			beacon = collision.gameObject.GetComponent<Beacon> ();
            beacon.Activate();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (dead) {
            // We're already dead. Please. No more.
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy.playerDetected &&
                hurtCooldownCount <= 0 &&
                dashCount <= DashInvincibleTime && 
                gameObject.layer != LayerMask.NameToLayer("Debris"))
            {
                playerHealth.value -= enemy.damage;
                cam.shakeDuration = 0.5f;
                StopAllCoroutines();
                StartCoroutine(SpriteColor(damageColor, HurtCooldownTime));

                if (playerHealth <= 0)
                {
                    _musicloop.otherSoundPlaying = true;
                    _deathaudio.Play();

                    dead = true;
                    playerHealth.value = 0;
                    _anim.SetTrigger("Die");
                    // Make the player not fly around as much
                    rb.drag = 50;
                }
                else
                {
                    _hurtaudio.Play();
                    hurtCooldownCount = HurtCooldownTime;
                }
            }
        }
    }

    IEnumerator SpriteColor(Color color, float totalTime)
    {
        float colorTimer = 0f;

        while (colorTimer < totalTime)
        {
            spriteRenderer.material.color = Color.Lerp(color, Color.white, colorTimer / totalTime);

            colorTimer += Time.deltaTime;
            yield return null;
        }
    }

    public override void Update ()
    {
        base.Update();

        if (inSpace)
        {
			
            return;
        }
        if (dead) {
            // Yep, no input here as well
            return;
        }

        if (hurtCooldownCount > 0) {
            hurtCooldownCount -= Time.deltaTime;
        }

        // State transitions
        switch (state) {
			case PlayerState.Walking:

				dashCooldownCount -= Time.deltaTime;
                if (Input.GetButtonDown("Dash") && dashCooldownCount <= 0)
                {
					_dashaudio.Play ();
                    state = PlayerState.Dashing;
                    dashCount = DashTime; // something?
                    StopAllCoroutines();
                    StartCoroutine(SpriteColor(invincibleColor, DashInvincibleTime));
                }
                break;
			case PlayerState.Dashing:
				dashCount -= Time.deltaTime;
                if (dashCount <= 0) {
                    state = PlayerState.Walking;
                    dashCooldownCount = DashCooldownTime;
                    // TODO? Update the player input so the gravity and such makes sense?
                }
                break;
        }

        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDir.sqrMagnitude > 0.4) {
            dashDir = moveDir.normalized;
        }

        // Walk SFX
        if (WalkSfxCount > 0) {
            WalkSfxCount -= Time.deltaTime;
        }
		if (moveDir.sqrMagnitude > 0.1) {
            if (WalkSfxCount <= 0) {
                WalkSfxCount = WalkSfxTime;
                _walkaudio.Play();
            }
		}

        _anim.SetFloat("MoveSpeed", moveDir.sqrMagnitude);
        _anim.SetBool("Roll", state == PlayerState.Dashing);

        // Instruction updating
        const int instructionDist = 10;
        if ((transform.position - lastInstructionPos).sqrMagnitude > instructionDist * instructionDist) {
            instructionHandler.AddNextPlayerInstruction(transform.position);
            lastInstructionPos = transform.position;
        }
	}

    private void FixedUpdate()
    {
        if (inSpace)
        {
            return;
        }
        if (dead) {
            // Dead guys don't get to move
            return;
        }

        switch (state)
        {
            case PlayerState.Walking:
                rb.MovePosition(rb.position + moveDir * Speed * Time.fixedDeltaTime);
                break;
            case PlayerState.Dashing:
                rb.MovePosition(rb.position + dashDir * DashSpeed * Time.fixedDeltaTime);
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

    public override void EnterSpace(Vector3 spaceTorque)
    {
        base.EnterSpace(spaceTorque);

        if (beaconsLit)
        {
            playerWonRef.value = true;
			_musicloop.otherSoundPlaying = true;
			_winaudio.Play ();

        }
        else
        {
            playerHealth.value = 0;
			if (!_spacedeath) {
				_musicloop.otherSoundPlaying = true;
				_spacedeathaudio.Play ();
				_spacedeath = true;
			}
        }
    }
}
