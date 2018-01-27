using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private CharacterController _controller;
	public float Speed = 2f;

	void Start() {
		_controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	    _controller.Move(move * Time.deltaTime * Speed);
	}
}
