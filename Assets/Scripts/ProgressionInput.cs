using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionInput : MonoBehaviour
{
    public IntReference playerHealth;
    public BoolReference playerWon;

    private Progression progressionManager;

    void Awake()
    {
        progressionManager = GameObject.FindGameObjectWithTag("Progression").GetComponent<Progression>();
    }

	void Update()
    {
        bool submitPressed = Input.GetButtonDown("Submit");
        bool resetPressed = Input.GetButtonDown("Reset");

        if (submitPressed && playerWon)
        {
            progressionManager.NextLevel();
        }
        else if ((submitPressed || resetPressed) && playerHealth <= 0 && !playerWon)
        {
			progressionManager.RestartLevel ();
		}
	}
}
