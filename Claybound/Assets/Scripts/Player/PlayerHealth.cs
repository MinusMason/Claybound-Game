using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth { get; private set; }


    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;

    private HitFlash hitFlash;
    private float nextFlashTime;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        hitFlash = GetComponent<HitFlash>();
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log("TakeDamage called! Health now: " + currentHealth);
        onHealthChanged?.Invoke(currentHealth / maxHealth);

        if (Time.time >= nextFlashTime)
        {
            hitFlash?.Flash();
            nextFlashTime = Time.time + 0.3f;
        }

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        onDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
