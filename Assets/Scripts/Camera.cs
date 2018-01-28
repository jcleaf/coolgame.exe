using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float cameraOffset;

    public bool moveToPlayer;

    public Transform title;

    public IntReference levelNumber;

    // How long the object should shake for.
	public float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
    private bool hadPlayer;
	private Vector3 shakeVert;
	private Vector3 shakeHoriz;

	void Awake(){
		shakeVert = Vector3.forward;
		shakeVert.y = 1.19175f;
		shakeHoriz = Vector3.right;
	}

    void Start()
    {
        if (levelNumber.value != 0)
        {
            moveToPlayer = true;
            Destroy(title.gameObject, 1f);
        }
    }

	// Update is called once per frame
	void FixedUpdate()
	{
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player && !moveToPlayer)
        {
            Vector3 newPos = player.transform.position - cameraOffset * transform.forward - Vector3.forward * 400f;
            transform.position = newPos;

            if (Input.GetButtonDown("Submit"))
            {
                moveToPlayer = true;
                title.SetParent(null);
                Destroy(title.gameObject, 1f);
            }

            hadPlayer = true;
        }

        if (player && moveToPlayer)
        {
            Vector3 newPos = player.transform.position - cameraOffset * transform.forward;

            if (!hadPlayer)
            {
                transform.position = newPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);
            }
            

            

			if (shakeDuration > 0) {
				Vector2 rand = Random.insideUnitCircle * shakeAmount;
				Vector3 newpos = rand.x * shakeHoriz + rand.y * shakeVert;
				transform.localPosition +=  newpos;

				shakeDuration -= Time.deltaTime * decreaseFactor;
			}

            hadPlayer = true;
        }
	}
}
