using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private IntReference playerHealth;

    private Text healthText;

    void Awake()
    {
        healthText = GetComponent<Text>();
    }

    void Update()
    {
        healthText.text = playerHealth.value.ToString() + "%";
    }
}
