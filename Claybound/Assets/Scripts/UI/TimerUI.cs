using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (GameTimer.Instance == null || timerText == null) return;

        float t = GameTimer.Instance.ElapsedTime;
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
