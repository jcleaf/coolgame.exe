using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField]
    [Tooltip("How much to move the sprite forward (to avoid clipping).")]
    private float Offset;

    private Vector3 StartPos;

    void Start()
    {
        StartPos = transform.localPosition;
    }

	void Update () {
        transform.LookAt(transform.position + UnityEngine.Camera.main.transform.rotation * Vector3.forward,
                    UnityEngine.Camera.main.transform.rotation * Vector3.up);
        Vector3 dir = new Vector3(transform.up.x, 0, transform.up.z);
        transform.localPosition = StartPos - Offset * dir;
	}
}
