using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Boss")]
    public GameObject bossPrefab;

    [Header("Trapdoor")]
    public GameObject trapdoorClosed;
    public GameObject trapdoorOpen;

    [Header("UI")]
    public GameObject levelCompletePanel;
    public bool isFinalLevel = false;

    private bool bossDefeated  = false;
    private bool bossSummoned  = false;
    private bool playerInRange = false;
    private bool playerEntered = false;

    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    private void Update()
    {
        // Summon boss when pressing E
        if (playerInRange && !bossSummoned && Input.GetKeyDown(KeyCode.E))
            SummonBoss();

        // Level complete once the boss is dead
        if (playerInRange && bossDefeated && !playerEntered)
        {
            playerEntered = true;
            DiceRollUI.Instance?.HidePrompt();
            ShowLevelComplete();
        }
    }

    private void SummonBoss()
    {
        bossSummoned = true;
        DiceRollUI.Instance?.HidePrompt();

        Vector3 spawnPos = transform.position + transform.forward * 5f;
        GameObject bossObj = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        Boss boss = bossObj.GetComponent<Boss>();
        if (boss != null) boss.portal = this;
    }

    public void OnBossDefeated()
    {
        bossDefeated = true;

        // Swap trapdoor to open
        if (trapdoorClosed != null) trapdoorClosed.SetActive(false);
        if (trapdoorOpen   != null) trapdoorOpen.SetActive(true);

        string prompt = isFinalLevel ? "Boss defeated!  Enter the trapdoor" : "Boss defeated!  Enter the trapdoor ... Level 2 awaits";
        DiceRollUI.Instance?.ShowPrompt(prompt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (!bossSummoned)
            DiceRollUI.Instance?.ShowPrompt("Press E to Summon the Boss");
        else if (bossDefeated && !playerEntered)
        {
            playerEntered = true;
            DiceRollUI.Instance?.HidePrompt();
            ShowLevelComplete();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (!bossDefeated)
            DiceRollUI.Instance?.HidePrompt();
    }

    private void ShowLevelComplete()
    {
        if (GameTimer.Instance != null)
            GameTimer.Instance.StopTimer();

        if (!isFinalLevel)
        {
            Time.timeScale   = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
            SceneManager.LoadScene("main 1");
            return;
        }

        Time.timeScale   = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        SceneManager.LoadScene(0);
    }
}
