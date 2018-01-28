using UnityEngine;
using UnityEngine.UI;

public class IntDisplayUI : MonoBehaviour
{
    [SerializeField] private IntReference intRef;
    [SerializeField] private bool showPercentage;

    private Text intText;

    void Awake()
    {
        intText = GetComponent<Text>();
    }

    void Update()
    {
        intText.text = intRef.value.ToString();

        if (showPercentage)
        {
            intText.text += "%";
        }
    }
}
