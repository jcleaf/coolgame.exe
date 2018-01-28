using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public IntReference playerHealth;
    public BoolReference playerWon;

    private Image goBackground;
    private Text goText;

    void Awake()
    {
        goBackground = GetComponentInChildren<Image>();
        goText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        goBackground.enabled = playerHealth <= 0 && !playerWon;
        goText.enabled = playerHealth <= 0 && !playerWon;
    }
}
