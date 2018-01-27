using UnityEngine;

public class SuckOut : MonoBehaviour
{
    public float suckStrength;
    public AnimationCurve distanceMultiplierCurve;
    private float triggerSize;

    void Awake()
    {
        SphereCollider triggerCollider = GetComponent<SphereCollider>();
        triggerSize = triggerCollider.radius * triggerCollider.transform.lossyScale.x;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            Vector3 offset = transform.position - other.transform.position;
            Vector3 suckDir = offset.normalized;
            float distanceMutliplier = distanceMultiplierCurve.Evaluate(offset.sqrMagnitude / (triggerSize * triggerSize));
            other.attachedRigidbody.AddForce(suckDir * suckStrength * distanceMutliplier * Time.fixedDeltaTime);
        }
    }
}
