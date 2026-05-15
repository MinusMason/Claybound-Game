using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30f;
    public int goldDrop = 5;
    private float currentHealth;
    private bool  isDead = false;
    private HitFlash hitFlash;
    private float nextFlashTime;

    private void Awake()
    {
        currentHealth = maxHealth;
        hitFlash = GetComponent<HitFlash>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
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
        isDead = true;
        GoldManager.Instance?.Add(goldDrop);
        GetComponent<enemy>()?.TriggerDeath();
        Destroy(gameObject, 2f); // delay to let death animation play
    }
}
