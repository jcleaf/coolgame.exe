using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject player;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void FixedUpdate()
	{
        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player) {
            Vector3 newPos = player.transform.position - 10 * transform.forward;
            transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
        }
	}
}
