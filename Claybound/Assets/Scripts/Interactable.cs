using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum StatType { Might, Finesse, Weird, Gab }

    [Header("Dice Roll Settings")]
    public StatType statRequired;
    public int difficultyValue = 7;

    [Header("Item Reward")]
    public string itemName = "Unknown Item";
    public StatType statToIncrease;
    public int increaseAmount = 2;
    public StatType statToDecrease;
    public int decreaseAmount = 1;

    private bool playerInRange = false;
    private bool hasBeenUsed = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (playerInRange && !hasBeenUsed && Input.GetKeyDown(KeyCode.E))
            AttemptRoll();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenUsed)
        {
            playerInRange = true;
            DiceRollUI.Instance?.ShowPrompt($"Press E  —  {statRequired.ToString().ToUpper()} Check  (Difficulty {difficultyValue})");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            DiceRollUI.Instance?.HidePrompt();
        }
    }

    private void AttemptRoll()
    {
        hasBeenUsed = true;
        DiceRollUI.Instance?.HidePrompt();

        animator?.SetTrigger("Open");

        int statValue = GetStatValue();
        int diceRoll  = Random.Range(1, 7); // d6 (can change to a higher range if needed)
        int total     = statValue + diceRoll;
        bool success  = total >= difficultyValue;

        if (success)
            ApplyItem();

        DiceRollUI.Instance?.ShowResult(
            statRequired.ToString(), statValue, diceRoll,
            total, difficultyValue, success,
            success ? itemName : ""
        );
    }

    private int GetStatValue()
    {
        if (PlayerStats.Instance == null) return 0;
        switch (statRequired)
        {
            case StatType.Might: return PlayerStats.Instance.might;
            case StatType.Finesse: return PlayerStats.Instance.finesse;
            case StatType.Weird: return PlayerStats.Instance.weird;
            case StatType.Gab: return PlayerStats.Instance.gab;
            default: return 0;
        }
    }

    private void ApplyItem()
    {
        PlayerStats.Instance.ModifyStat(statToIncrease.ToString().ToLower(), increaseAmount);
        PlayerStats.Instance.ModifyStat(statToDecrease.ToString().ToLower(), -decreaseAmount);
    }
}
