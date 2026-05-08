using UnityEngine;
using TMPro;
using System.Collections;

public class DiceRollUI : MonoBehaviour
{
    public static DiceRollUI Instance { get; private set; }

    [Header("Prompt")]
    public GameObject promptPanel;
    public TextMeshProUGUI promptText;

    [Header("Result")]
    public GameObject resultPanel;
    public TextMeshProUGUI diceText;
    public TextMeshProUGUI resultText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (promptPanel != null) promptPanel.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);
    }

    public void ShowPrompt(string message)
    {
        if (promptPanel == null) return;
        promptPanel.SetActive(true);
        promptText.text = message;
    }

    public void HidePrompt()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false);
    }

    public void ShowResult(string stat, int statVal, int roll, int total, int difficulty, bool success, string itemName)
    {
        StartCoroutine(ResultCoroutine(stat, statVal, roll, total, difficulty, success, itemName));
    }

    private IEnumerator ResultCoroutine(string stat, int statVal, int roll, int total, int difficulty, bool success, string itemName)
    {
        resultPanel.SetActive(true);
        resultText.text = "";

        // Animate dice cycling
        diceText.text = "Rolling...";
        for (int i = 0; i < 12; i++)
        {
            diceText.text = Random.Range(1, 7).ToString();
            yield return new WaitForSeconds(0.07f);
        }

        // Show final roll
        diceText.text = roll.ToString();

        // Show breakdown
        string colour  = success ? "#55FF55" : "#FF4444";
        string outcome = success ? $"SUCCESS!\n<size=70%>+{increaseText(itemName)}</size>" : "FAILURE";

        resultText.text = $"{stat.ToUpper()}  {statVal}  +  Roll  {roll}  =  {total}  vs  {difficulty}\n" +
                          $"<color={colour}>{outcome}</color>";

        yield return new WaitForSeconds(3.5f);
        resultPanel.SetActive(false);
    }

    // Display the item name
    private string increaseText(string itemName) => string.IsNullOrEmpty(itemName) ? "" : itemName;
}
