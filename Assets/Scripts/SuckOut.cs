using UnityEngine;
using UnityEngine.AI;

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
        if (other.attachedRigidbody)
        {
            Vector3 offset = transform.position - other.transform.position;
            Vector3 suckDir = Vector3.ProjectOnPlane(offset, Vector3.up).normalized;
            float triggerSize = triggerCollider.radius * triggerCollider.transform.lossyScale.x;

            float distanceMutliplier = distanceMultiplierCurve.Evaluate(offset.sqrMagnitude / (triggerSize * triggerSize));

            NavMeshAgent agent = other.gameObject.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }

            other.attachedRigidbody.isKinematic = false;
            other.attachedRigidbody.drag = 0f;
            other.attachedRigidbody.AddForce(suckDir * suckStrength * distanceMutliplier * Time.fixedDeltaTime);
        }
    }
}
