using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public IntReference playerHealth;

	void Update ()
    {
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Reset")) && playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}
}
