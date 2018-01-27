using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	
	void Update () {
        transform.LookAt(transform.position + UnityEngine.Camera.main.transform.rotation * Vector3.forward,
                    UnityEngine.Camera.main.transform.rotation * Vector3.up);
	}
}
