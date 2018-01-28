using UnityEngine;

/**
 * Shared stuff for players and enemies
 */
public class MovingObject : MonoBehaviour
{
    protected bool inSpace;

    private Rigidbody rb;
    private Billboard billboard;
    [SerializeField]
    private Transform graphics;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        billboard = GetComponentInChildren<Billboard>();
    }

    public virtual Vector3 GetMoveDir()
    {
        return Vector3.zero;
    }

    public virtual void Update()
    {
        Vector3 moveDir = GetMoveDir();

        // Update facing direction
        if (moveDir.x > 0.1)
        {
            Quaternion newDir = Quaternion.AngleAxis(0, Vector3.up);
            graphics.localRotation = Quaternion.Lerp(graphics.localRotation, newDir, 0.1f);
        }
        else if (moveDir.x < -0.1) {
            Quaternion newDir = Quaternion.AngleAxis(180, Vector3.up);
            graphics.localRotation = Quaternion.Lerp(graphics.localRotation, newDir, 0.1f);
        }
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
