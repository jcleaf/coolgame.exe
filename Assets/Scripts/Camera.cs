using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float cameraOffset;

    private bool hadPlayer;
    // How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	private Vector3 shakeVert;
	private Vector3 shakeHoriz;

	void Awake(){
		shakeVert = Vector3.forward;
		shakeVert.y = 1.19175f;
		shakeHoriz = Vector3.right;
	}
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

			if (shakeDuration > 0) {
				Vector2 rand = Random.insideUnitCircle * shakeAmount;
				Vector3 newpos = rand.x * shakeHoriz + rand.y * shakeVert;
				transform.localPosition +=  newpos;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			} 
        }


	}




}
