using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Slider healthSlider;
    private PlayerHealth playerHealth;

    private void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        if (healthSlider == null)
            healthSlider = FindObjectOfType<Slider>();

        playerHealth = FindObjectOfType<PlayerHealth>();

        if (healthSlider != null)
            healthSlider.value = 1f;

        Debug.Log("HealthBarUI found Slider: " + (healthSlider != null) + ", PlayerHealth: " + (playerHealth != null));
    }

    private float logTimer;

    private void Update()
    {
        if (playerHealth == null || healthSlider == null) return;

        float normalised = playerHealth.currentHealth / playerHealth.maxHealth;
        healthSlider.value = normalised;

        logTimer += Time.deltaTime;
        if (logTimer >= 1f)
        {
            Debug.Log("Health: " + playerHealth.currentHealth + " / " + playerHealth.maxHealth + " | Slider value: " + healthSlider.value);
            logTimer = 0f;
        }
    }
}
