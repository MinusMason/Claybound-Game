using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBarUI : MonoBehaviour
{
    public GameObject bossHealthPanel;
    public Slider healthSlider;
    public TextMeshProUGUI bossLabel;

    private void Start()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }

    public void SetBoss(Boss boss)
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);

        if (healthSlider != null)
            healthSlider.value = 1f;

        if (bossLabel != null)
            bossLabel.text = "The Boss";
    }

    public void UpdateHealth(float normalised)
    {
        if (healthSlider != null)
            healthSlider.value = normalised;

        if (normalised <= 0f && bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }
}
