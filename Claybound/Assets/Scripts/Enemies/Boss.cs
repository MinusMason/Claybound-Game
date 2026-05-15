using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("Phase 1 (full health -> 50%)")]
    public float phase1Speed  = 3f;
    public float phase1Damage = 10f;
    public float phase1AttackInterval = 3f;

    [Header("Phase 2 (below 50%)")]
    public float phase2Speed  = 6f;
    public float phase2Damage = 20f;
    public float phase2AttackInterval = 1.8f;

    [Header("Rewards")]
    public int goldDrop = 50;

    [Header("References")]
    private BossHealthBarUI healthBar;
    [HideInInspector] public Portal portal;

    private Rigidbody    rb;
    private Animator     animator;
    private HitFlash     hitFlash;
    private float        nextFlashTime;
    private Transform    playerTransform;
    private PlayerHealth playerHealth;

    private bool  inPhase2  = false;
    private bool  isDead    = false;
    private float currentSpeed;
    private float currentDamage;
    private float attackTimer;
    private float currentAttackInterval;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        hitFlash = GetComponentInChildren<HitFlash>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        currentHealth = maxHealth;
        currentSpeed  = phase1Speed;
        currentDamage = phase1Damage;
        currentAttackInterval = phase1AttackInterval;

        healthBar = FindFirstObjectByType<BossHealthBarUI>(FindObjectsInactive.Include);
        if (healthBar != null) healthBar.SetBoss(this);

        if (rb != null) rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (isDead || playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;
        rb.linearVelocity = new Vector3(direction.x * currentSpeed, rb.linearVelocity.y, direction.z * currentSpeed);

        if (direction != Vector3.zero)
            transform.forward = direction;

        // Attack timer
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer >= currentAttackInterval)
        {
            attackTimer = 0f;
            TriggerAttack();
        }

        // Damage player if in range
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= 3f && playerHealth != null)
            playerHealth.TakeDamage(currentDamage * Time.fixedDeltaTime);
    }

    private void TriggerAttack()
    {
        if (inPhase2)
            animator?.SetTrigger("attack1");
        else
            animator?.SetTrigger("attack2");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth  = Mathf.Max(0f, currentHealth);

        animator?.SetTrigger("hit");

        if (Time.time >= nextFlashTime)
        {
            hitFlash?.Flash();
            nextFlashTime = Time.time + 0.1f;
        }

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth / maxHealth);

        if (!inPhase2 && currentHealth <= maxHealth * 0.5f)
        {
            inPhase2 = true;
            currentSpeed = phase2Speed;
            currentDamage = phase2Damage;
            currentAttackInterval = phase2AttackInterval;
        }

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector3.zero;

        animator?.SetTrigger("death");

        GoldManager.Instance?.Add(goldDrop);
        if (healthBar != null) healthBar.UpdateHealth(0f);

        portal?.OnBossDefeated();

        Destroy(gameObject, 3f);
    }
}
