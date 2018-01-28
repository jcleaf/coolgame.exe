using UnityEngine;

public class SuckOut : MonoBehaviour
{
    public float suckStrength;
    public AnimationCurve distanceMultiplierCurve;

    private SphereCollider triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<SphereCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        MovingObject movingObject = other.GetComponent<MovingObject>();
        if (movingObject != null)
        {
            Vector3 offset = transform.position - other.transform.position;
            Vector3 suckDir = Vector3.ProjectOnPlane(offset, Vector3.up).normalized;
            float triggerSize = triggerCollider.radius * triggerCollider.transform.lossyScale.x;
            float distanceMutliplier = distanceMultiplierCurve.Evaluate(offset.sqrMagnitude / (triggerSize * triggerSize));

            movingObject.BeingSucked(suckDir * suckStrength * distanceMutliplier * Time.fixedDeltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        MovingObject movingObject = other.GetComponent<MovingObject>();
        if (movingObject != null)
        {
            movingObject.StopBeingSucked();
        }
    }
}
