using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float rotationSpeed = 720f;

    private Rigidbody rb;
    private Animator animator;
    private Camera cam;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Camera-relative directions flattened onto the horizontal plane
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        moveDir = (camForward * v + camRight * h).normalized;

        animator.SetBool("isRunning", moveDir.magnitude > 0.1f);
    }

    private void FixedUpdate()
    {
        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        float finesseMult = PlayerStats.Instance != null ? 1f + (PlayerStats.Instance.finesse - 1) * 0.15f : 1f;
        rb.MovePosition(rb.position + moveDir * moveSpeed * finesseMult * Time.fixedDeltaTime);
    }
}
