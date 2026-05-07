using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.onDeath.AddListener(ShowGameOver);
    }

    private void ShowGameOver()
    {
        if (GameTimer.Instance != null)
            GameTimer.Instance.StopTimer();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
