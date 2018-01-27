using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private bool openCheckbox;
    [SerializeField] private BoxCollider doorCollider;

    [HideInInspector] public Level levelManager;

    private bool open;
    private GameObject suckOut;

    void Awake()
    {
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

        other.gameObject.layer = LayerMask.NameToLayer("Debris");

        if (other.gameObject.tag == "Player" && levelManager.levelFinished)
        {
            //TODO: YOU WIN!!!
        }
    }

    public void Open() { ToggleOpenState(true); }

    public void Close() { ToggleOpenState(false); }

    private void ToggleOpenState(bool open)
    {
        this.open = open;
        openCheckbox = open;

        suckOut.SetActive(open);
        doorCollider.gameObject.SetActive(!open);
    }
}
