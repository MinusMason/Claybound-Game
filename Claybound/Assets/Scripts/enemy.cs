using UnityEngine;

public class enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Transform playerTransform;

    public void Initialize(Transform player)
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = player;

        // Ensure the Rigidbody doesn't tip over
        rb.freezeRotation = true;
    }

    
    public void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        // Calculate direction to the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Only want to move horizontally,
        direction.y = 0;

        // Apply velocity towards the player
        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);

        // Make the enemy face the player
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }
}
