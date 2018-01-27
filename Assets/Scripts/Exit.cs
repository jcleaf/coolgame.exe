using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.layer = LayerMask.NameToLayer("Debris");
    }
}
