using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private CharacterController _controller;

    public float Speed = 10f;
    public float DashSpeed = 30f;
    public float DashTime = 0.2f;
    public float DashCooldown = 0.5f;

    public PlayerState state;
    private float dashCount = 0;
    private float dashCooldown = 0;

    // Some default direction
    private Vector3 dashDir = new Vector3(1, 0, 0);

    public enum PlayerState {
        Walking, Dashing
    }

	void Start() {
		_controller = GetComponent<CharacterController>();
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

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveDir.sqrMagnitude > 0.4) {
            dashDir = moveDir.normalized;
        }
        switch (state) {
            case PlayerState.Walking:
                _controller.Move(moveDir * Time.deltaTime * Speed);
                break;
            case PlayerState.Dashing:
                _controller.Move(dashDir * Time.deltaTime * DashSpeed);
                break;
        }
	}
}
