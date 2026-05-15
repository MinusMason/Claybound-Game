using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed    = 5f;
    public float damagePerSecond = 5f;
    public float damageRange  = 2.5f;

    private Rigidbody  rb;
    private Transform  playerTransform;
    private PlayerHealth playerHealth;
    private Animator   animator;

    public bool isDead       = false;
    private bool isSpawning  = true;
    private float spawnTimer = 0f;
    public float spawnDuration = 1.5f; 
    public void Initialize(Transform player)
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = player;
        playerHealth = player.GetComponent<PlayerHealth>();
        rb.freezeRotation = true;

        animator = GetComponentInChildren<Animator>();
    }

    public void MoveTowardsPlayer()
    {
        if (playerTransform == null || isDead) return;

        // Rise up from the ground during spawn animation
        if (isSpawning)
        {
            spawnTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(spawnTimer / spawnDuration);
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(-1f, 0f, t);
            transform.position = pos;
            if (spawnTimer >= spawnDuration)
                isSpawning = false;
            return;
        }

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;

        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);

        if (direction != Vector3.zero)
            transform.forward = direction;

        // Animate running
        animator?.SetBool("isRunning", true);

        // Damage player if close enough
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= damageRange && playerHealth != null)
            playerHealth.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
    }

    public void TriggerDeath()
    {
        if (isDead) return;
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        animator?.SetBool("isRunning", false);
        animator?.SetTrigger("death");
    }
}
