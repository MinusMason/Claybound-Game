using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float ElapsedTime { get; private set; }

    public float DifficultyMultiplier { get; private set; } = 1f;

    [Header("Difficulty Scaling")]
    [Tooltip("How many seconds it takes for difficulty to double")]
    public float rampTime = 60f;

    // Carries over the time
    private static float s_elapsedTime = 0f;

    private bool isRunning = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Resume from wherever the previous level ended
        ElapsedTime = s_elapsedTime;
        DifficultyMultiplier = 1f + (ElapsedTime / rampTime);
    }

    private void Update()
    {
        if (!isRunning) return;

        ElapsedTime += Time.deltaTime;
        s_elapsedTime = ElapsedTime;
        DifficultyMultiplier = 1f + (ElapsedTime / rampTime);
    }

    public void StopTimer() => isRunning = false;

    public static void ResetStatics() { s_elapsedTime = 0f; }
}
