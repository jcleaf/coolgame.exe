using UnityEngine;
using UnityEngine.UI;

public class YouWinUI : MonoBehaviour
{
    public BoolReference youWinRef;

    private Image background;
    private Text[] texts;

    private float graphicsCount = -1;

    void Awake()
    {
        background = GetComponentInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    void Update()
    {
        var done = youWinRef;
        background.enabled = done;
        foreach (Text text in texts)
        {
            text.enabled = done;
        }

        if (done)
        {
            graphicsCount += 0.5f * Time.deltaTime;
            if (graphicsCount > 1)
            {
                graphicsCount = 1;
            }

            var c = background.color;
            c.a = 0.5f * graphicsCount;
            background.color = c;

            foreach (Text text in texts)
            {
                var c2 = text.color;
                c2.a = 1f * graphicsCount;
                text.color = c2;
            }
        }
    }
}
