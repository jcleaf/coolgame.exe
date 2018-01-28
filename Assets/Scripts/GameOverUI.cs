using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public IntReference playerHealth;
    public BoolReference playerWon;

    private Image goBackground;
    private Text[] texts;

    private float graphicsCount = -1;

    void Awake()
    {
        goBackground = GetComponentInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    void Update()
    {
        var done = (playerHealth <= 0 && !playerWon);
        goBackground.enabled = done;
        foreach (Text text in texts)
        {
            text.enabled = done;
        }

        if (done) {
            graphicsCount += 0.5f * Time.deltaTime;
            if (graphicsCount > 1)
            {
                graphicsCount = 1;
            }

            var c = goBackground.color;
            c.a = 0.5f * graphicsCount;
            goBackground.color = c;

            foreach (Text text in texts)
            {
                var c2 = text.color;
                c2.a = 1f * graphicsCount;
                text.color = c2;
            }
        }
    }
}
