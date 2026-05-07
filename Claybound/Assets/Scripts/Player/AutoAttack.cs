using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float baseDamage = 10f;
    public float attackRate = 1f;      // Attacks per second
    public int projectileCount = 1;    // Items can increase this

    [Header("Range")]
    public float attackRange = 15f;

    private float attackTimer;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= 1f / attackRate)
        {
            TryAttack();
            attackTimer = 0f;
        }
    }

    private void TryAttack()
    {
        enemy[] enemies = FindObjectsByType<enemy>(FindObjectsSortMode.None);
        if (enemies.Length == 0) return;

        // Find nearest enemy
        enemy nearest = null;
        float minDist = attackRange;
        foreach (enemy e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }

        if (nearest == null) return;

        // Scale damage with Might stat
        float damage = baseDamage;
        if (PlayerStats.Instance != null)
            damage = baseDamage * (1f + (PlayerStats.Instance.might - 1) * 0.2f);

        // Fire projectiles — if count > 1, spread them in a small arc
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1f;
            Vector3 targetPos = nearest.transform.position + Vector3.up * 0.5f;
            Vector3 baseDir = (targetPos - spawnPos).normalized;

            if (projectileCount > 1)
            {
                float spread = (i - (projectileCount - 1) / 2f) * 15f;
                baseDir = Quaternion.Euler(0, spread, 0) * baseDir;
            }

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(baseDir));
            proj.GetComponent<Projectile>()?.Init(baseDir, damage);
        }
    }
}
