using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    private TextMeshProUGUI goldText;

    private void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();

        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.onGoldChanged.AddListener(UpdateDisplay);
            UpdateDisplay(GoldManager.Instance.Gold);
        }
    }

    private void UpdateDisplay(int amount)
    {
        if (goldText != null)
            goldText.text = $"Gold: {amount}";
    }
}
