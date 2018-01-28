using UnityEngine;
using UnityEngine.UI;

public class YouWinUI : MonoBehaviour
{
    public BoolReference youWinRef;

    private Image background;
    private Text[] texts;

    void Awake()
    {
        background = GetComponentInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    void Update()
    {
        background.enabled = youWinRef;
        foreach (Text text in texts) { text.enabled = youWinRef; };
    }
}
