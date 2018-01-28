using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public IntReference playerHealth;
    public BoolReference playerWon;

    private Image goBackground;
    private Text[] texts;

    void Awake()
    {
        goBackground = GetComponentInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    void Update()
    {
        goBackground.enabled = playerHealth <= 0 && !playerWon;
        foreach (Text text in texts) { text.enabled = playerHealth <= 0 && !playerWon; }
    }
}
