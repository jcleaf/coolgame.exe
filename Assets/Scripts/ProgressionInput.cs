using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionInput : MonoBehaviour
{
    public IntReference playerHealth;
    public BoolReference playerWon;

	void Update()
    {
        bool submitPressed = Input.GetButtonDown("Submit");
        bool resetPressed = Input.GetButtonDown("Reset");

        if (submitPressed && playerWon)
        {
            //TODO: progress to next level
            Debug.Log("PROGRESS");
        }
        else if ((submitPressed || resetPressed) && playerHealth <= 0 && !playerWon)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
