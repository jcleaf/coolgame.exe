using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody _body;

    public float Speed = 10f;
    public float DashSpeed = 30f;
    public float DashTime = 0.2f;
    public float DashCooldown = 0.5f;

    public PlayerState state;
    private float dashCount = 0;
    private float dashCooldown = 0;

    // Some default direction
    private Vector3 moveDir = Vector3.zero;
    private Vector3 dashDir = new Vector3(1, 0, 0);

    public enum PlayerState {
        Walking, Dashing
    }

	void Start() {
        _body = GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Beacon")
        {
            // TODO: Trigger the beacon
            Debug.Log("We're touching it");
            _body.AddForce(10 * Vector3.up, ForceMode.VelocityChange);
        }
    }

   void Update () {
        // State transitions
        switch (state) {
            case PlayerState.Walking:
                dashCooldown -= Time.deltaTime;
                if (Input.GetButtonDown("Dash") && dashCooldown < 0)
                {
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
	}

    private void FixedUpdate()
    {
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
}
