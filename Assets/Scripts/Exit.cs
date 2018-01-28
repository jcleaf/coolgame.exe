using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private bool openCheckbox;
    [SerializeField] private BoxCollider doorCollider;
    [SerializeField] private float spaceTorqueStrength;
    [SerializeField] private float doorTime;

    [SerializeField] private Door leftDoor;
    [SerializeField] private Door rightDoor;

    [HideInInspector] public Level levelManager;

	private AudioSource _dooraudio;
    [Serializable]
    private struct Door
    {
        public Transform transform;
        public Transform closed;
        public Transform open;
    }

    private bool open;
    public bool IsOpen { get { return open; } }
    private GameObject suckOut;
    private float doorTimer;

    void Awake()
    {
		_dooraudio = GetComponent<AudioSource> ();
        suckOut = GetComponentInChildren<SuckOut>().gameObject;
        suckOut.SetActive(false);
    }

    void Update()
    {
        if (open != openCheckbox)
        {
            ToggleOpenState(openCheckbox);
        }

        doorTimer += (1f / doorTime) * Time.deltaTime * (open ? 1f : -1f);
        doorTimer = Mathf.Clamp01(doorTimer);
        LerpDoor(leftDoor);
        LerpDoor(rightDoor);
    }

    void LerpDoor(Door door)
    {
        door.transform.position = Vector3.Lerp(door.closed.position, door.open.position, doorTimer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!open)
        {
            return;
        }

        MovingObject movingObject = other.GetComponent<MovingObject>();
        if (movingObject != null)
        {
            Vector3 spaceTorque = (new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value)).normalized * spaceTorqueStrength;
            movingObject.EnterSpace(spaceTorque);
        }
    }

    public void Open()
    {
        ToggleOpenState(true);
    }

    public void Close()
    {
        ToggleOpenState(false);
    }

    private void ToggleOpenState(bool open)
    {
		if (this.open != open) {
			_dooraudio.Play();
		}
        this.open = open;
        openCheckbox = open;

        suckOut.SetActive(open);
        doorCollider.gameObject.SetActive(!open);
    }
}
