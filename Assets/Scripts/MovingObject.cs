using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected bool inSpace;

    private Rigidbody rb;
    private Billboard billboard;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        billboard = GetComponentInChildren<Billboard>();
    }

    public virtual void BeingSucked(Vector3 suckForce)
    {
        rb.isKinematic = false;
        rb.drag = 0f;
        rb.AddForce(suckForce);
    }

    public virtual void StopBeingSucked() { }

    public virtual void EnterSpace(Vector3 spaceTorque)
    {
        inSpace = true;

        gameObject.layer = LayerMask.NameToLayer("Debris");

        rb.isKinematic = false;
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(spaceTorque, ForceMode.Impulse);

        if (billboard != null)
        {
            billboard.enabled = false;
        }
    }
}
