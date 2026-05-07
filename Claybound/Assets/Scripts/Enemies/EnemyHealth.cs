using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    private float currentHealth;
    private HitFlash hitFlash;
    private float nextFlashTime;

    private void Awake()
    {
        currentHealth = maxHealth;
        hitFlash = GetComponent<HitFlash>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (Time.time >= nextFlashTime)
        {
            hitFlash?.Flash();
            nextFlashTime = Time.time + 0.1f;
        }

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
