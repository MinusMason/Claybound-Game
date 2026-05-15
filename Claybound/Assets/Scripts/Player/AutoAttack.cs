using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;

    [Header("Stats")]
    public float baseDamage = 5f;
    public float attackRate = 1f; // Attacks per second
    public int projectileCount = 1; // Certain ttems can increase this

    [Header("Range")]
    public float attackRange = 15f;

    // Persists across scene loads so that shop upgrades carry over
    private static float s_extraDamage = 0f;
    private static int   s_extraProjectiles = 0;

    private float attackTimer;

    private void Start()
    {
        // Reapply any previous upgrades in new level
        baseDamage += s_extraDamage;
        projectileCount += s_extraProjectiles;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        // Weird scales attack speed
        float weirdMult = PlayerStats.Instance != null ? 1f + (PlayerStats.Instance.weird - 1) * 0.15f : 1f;
        float scaledRate = attackRate * weirdMult;

        if (attackTimer >= 1f / scaledRate)
        {
            TryAttack();
            attackTimer = 0f;
        }
    }

    public static void ResetStatics() { s_extraDamage = 0f; s_extraProjectiles = 0; }

    public void ModifyWeapon(int projectileBonus, float damageBonus)
    {
        projectileCount += projectileBonus;
        baseDamage += damageBonus;
        s_extraProjectiles += projectileBonus;
        s_extraDamage += damageBonus;
    }

    private void TryAttack()
    {
        // Scale damage with Might stat
        float damage = baseDamage;
        if (PlayerStats.Instance != null)
            damage = baseDamage * (1f + (PlayerStats.Instance.might - 1) * 0.2f);

        Vector3 targetPos = GetNearestTargetPosition();
        if (targetPos == Vector3.zero) return;

        // Fire projectiles
        for (int i = 0; i < projectileCount; i++)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1f;
            Vector3 baseDir  = (targetPos - spawnPos).normalized;
            // if count > 1 then spread in a small arc
            if (projectileCount > 1)
            {
                float spread = (i - (projectileCount - 1) / 2f) * 15f;
                baseDir = Quaternion.Euler(0, spread, 0) * baseDir;
            }

            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(baseDir));
            proj.GetComponent<Projectile>()?.Init(baseDir, damage);
        }
    }

    private Vector3 GetNearestTargetPosition()
    {
        float minDist = attackRange;
        Vector3 bestTarget = Vector3.zero;

        // Check normal enemies
        foreach (enemy e in FindObjectsByType<enemy>(FindObjectsSortMode.None))
        {
            if (e.isDead) continue;
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                bestTarget = e.transform.position + Vector3.up * 0.5f;
            }
        }

        // Check boss 
        Boss boss = FindFirstObjectByType<Boss>();
        if (boss != null && !boss.isDead)
        {
            float dist = Vector3.Distance(transform.position, boss.transform.position);
            if (dist < minDist)
                bestTarget = boss.transform.position + Vector3.up * 0.5f;
        }

        return bestTarget;
    }
}
