using UnityEngine;
using System.Collections;

public class DoorInstructionTrigger : MonoBehaviour
{

    private InstructionHandler instructionHandler;
    private Exit exit;
    private bool lastOpenStatus = false;

	// Use this for initialization
	void Awake()
	{
        instructionHandler = GameObject.Find("InstructionHandler").GetComponent<InstructionHandler>();
        exit = GetComponentInParent<Exit>();
	}

    private void OnTriggerEnter(Collider other)
    {
        // Try update instruction
        if (other.gameObject.tag == "Player")
        {
            instructionHandler.AddAirlockInstruction(exit.IsOpen ? 1 : 0, transform.position);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Try update instruction
        if (other.gameObject.tag == "Player")
        {
            if (lastOpenStatus != exit.IsOpen) {
                instructionHandler.AddAirlockInstruction(exit.IsOpen ? 1 : 0, transform.position);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Instructions
        if (other.gameObject.tag == "Player")
        {
            if (exit.IsOpen)
            {
                // Somewhat ugly hack to make it so that it doesn't show stuff again.
                instructionHandler.AddAirlockInstruction(2, transform.position);
            }
            instructionHandler.ClearAirlockInstruction();
        }
    }
}
