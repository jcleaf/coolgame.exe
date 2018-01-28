using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public IntReference playerHealth;

    private Image goBackground;
    private Text goText;

    void Awake()
    {
        goBackground = GetComponentInChildren<Image>();
        goText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        goBackground.enabled = playerHealth <= 0;
        goText.enabled = playerHealth <= 0;
    }
}
