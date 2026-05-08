using UnityEngine;

public class AnimationFrameRate : MonoBehaviour
{
    [Range(4, 30)]]
    public int framesPerSecond = 12;

    private Animator animator;
    private float timer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    private void Update()
    {
        float interval = 1f / framesPerSecond;
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // Advance the animation by exactly one frame and then freeze
            animator.speed = interval / Time.deltaTime;
            timer -= interval;
        }
        else
        {
            animator.speed = 0f;
        }
    }
}
