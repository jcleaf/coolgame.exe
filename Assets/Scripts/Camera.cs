using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float cameraOffset;

    private bool hadPlayer;

	// Update is called once per frame
	void FixedUpdate()
	{
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player)
        {
            Vector3 newPos = player.transform.position - cameraOffset * transform.forward;

            if (hadPlayer)
            {
                transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
            }
            else
            {
                transform.position = newPos;
            }
            
            hadPlayer = true;
        }
	}
}
