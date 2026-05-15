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

        Time.timeScale   = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    public void Restart()
    {
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        PlayerStats.ResetStatics();
        GoldManager.ResetStatics();
        AutoAttack.ResetStatics();
        GameTimer.ResetStatics();

        SceneManager.LoadScene(1); // Always restart from level 1
    }
}
