using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    public float ElapsedTime { get; private set; }


    public float DifficultyMultiplier { get; private set; } = 1f;

    [Header("Difficulty Scaling")]
    [Tooltip("How many seconds it takes for difficulty to double")]
    public float rampTime = 60f;

    private bool isRunning = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!isRunning) return;

        ElapsedTime += Time.deltaTime;
        DifficultyMultiplier = 1f + (ElapsedTime / rampTime);
    }

    public void StopTimer() => isRunning = false;
}
