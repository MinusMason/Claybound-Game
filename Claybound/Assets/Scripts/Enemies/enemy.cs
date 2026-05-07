using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float damagePerSecond = 5f;
    public float damageRange = 2.5f;

    private Rigidbody rb;
    private Transform playerTransform;
    private PlayerHealth playerHealth;

    public void Initialize(Transform player)
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = player;
        playerHealth = player.GetComponent<PlayerHealth>();
        rb.freezeRotation = true;
    }

    public void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0;

        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);

        if (direction != Vector3.zero)
            transform.forward = direction;

        // Damage player if close enough 
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= damageRange && playerHealth != null)
            playerHealth.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
    }
}
