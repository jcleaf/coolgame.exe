using UnityEngine;
using System.Collections;

public class BeaconInstruction : MonoBehaviour
{
    private InstructionHandler instructionHandler;
    private Beacon beacon;
    private bool lastOpenStatus = false;

    // Use this for initialization
    void Awake()
    {
        instructionHandler = GameObject.Find("InstructionHandler").GetComponent<InstructionHandler>();
        beacon = GetComponentInParent<Beacon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try update instruction
        if (other.gameObject.tag == "Player")
        {
            instructionHandler.AddBeaconInstruction(beacon.activated ? 1 : 0, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Try update instruction
        if (other.gameObject.tag == "Player")
        {
            if (lastOpenStatus != beacon.activated)
            {
                instructionHandler.AddBeaconInstruction(beacon.activated ? 1 : 0, transform.position);
                lastOpenStatus = beacon.activated;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Instructions
        if (other.gameObject.tag == "Player")
        {
            if (beacon.activated)
            {
                // Somewhat ugly hack to make it so that it doesn't show stuff again.
                instructionHandler.AddBeaconInstruction(2, transform.position);
            }
            instructionHandler.ClearBeaconInstruction();
        }
    }
}
