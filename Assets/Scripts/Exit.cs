using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private bool openCheckbox;
    [SerializeField] private BoxCollider doorCollider;
    [SerializeField] private float spaceTorqueStrength;

    [HideInInspector] public Level levelManager;

	private AudioSource _dooraudio;
    private bool open;
    private GameObject suckOut;

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
            Vector3 spaceTorque = (new Vector3(Random.value, Random.value, Random.value)).normalized * spaceTorqueStrength;
            movingObject.EnterSpace(spaceTorque);
        }
    }

    public void Open() { ToggleOpenState(true); }

    public void Close() { ToggleOpenState(false); }

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
