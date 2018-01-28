using UnityEngine;
using System.Collections;

public class InstructionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject instructionPrefab;

    private int playerIndex = 0;
    private int airlockIndex = 0;
    private int beaconIndex = 0;

    private string[] playerWords = {
        "Agent Zeta. If you can you hear me, press the arrow keys to move.",
        "Is your dash tech still active? Shift to trigger it.",
        "Activate all the beacons so we can locate your position.",
        "When they're all activated, jump out of one of the airlocks. We'll take you to the next ship.",
    };
    private string[] airlockWords = {
        "Airlocks active when you get near them. Which is... questionable design.",
        "Don't get sucked out!"
    };
    private string[] beaconWords = {
        "You need to touch the beacon to trigger it, but you'll also alert nearby enemies."
    };
    private string allBeaconWords = "All beacons activated. Make your way to an airlock.";

    public void AddNextPlayerInstruction(Vector3 pos) {
        if (playerIndex < playerWords.Length) {
            AddInstruction(playerWords[playerIndex], pos);
        }
        playerIndex ++;
    }

    public void AddInstruction(string word, Vector3 position) {
        GameObject go = Instantiate(instructionPrefab, position, Quaternion.identity);
    }
}
