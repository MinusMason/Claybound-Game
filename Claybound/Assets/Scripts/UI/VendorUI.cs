using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class VendorUI : MonoBehaviour
{
    public static VendorUI Instance { get; private set; }

    [System.Serializable]
    public class VendorItem
    {
        public string itemName;
        [TextArea] public string description;
        public int cost;
        public string statToIncrease;
        public int increaseAmount;
        public string statToDecrease;
        public int decreaseAmount;
    }

    [Header("Items for Sale")]
    public List<VendorItem> items = new List<VendorItem>()
    {
        new VendorItem { itemName = "Clay Fists",      description = "+2 MIGHT, -1 FINESSE",  cost = 15, statToIncrease = "might",   increaseAmount = 2, statToDecrease = "finesse", decreaseAmount = 1 },
        new VendorItem { itemName = "Quickstep Boots", description = "+2 FINESSE, -1 MIGHT",  cost = 15, statToIncrease = "finesse", increaseAmount = 2, statToDecrease = "might",   decreaseAmount = 1 },
        new VendorItem { itemName = "Chaos Shard",     description = "+2 WEIRD, -1 GAB",      cost = 15, statToIncrease = "weird",   increaseAmount = 2, statToDecrease = "gab",     decreaseAmount = 1 },
        new VendorItem { itemName = "Lucky Coin",      description = "+2 GAB, -1 WEIRD",      cost = 15, statToIncrease = "gab",     increaseAmount = 2, statToDecrease = "weird",   decreaseAmount = 1 },
    };

    [Header("UI References")]
    public GameObject shopPanel;
    public Transform itemListParent;
    public GameObject itemButtonPrefab;
    public TextMeshProUGUI feedbackText;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void Open()
    {
        if (shopPanel == null) return;
        shopPanel.SetActive(true);
        IsOpen = true;
        BuildItemList();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (feedbackText != null) feedbackText.text = "";
    }

    public void Close()
    {
        if (shopPanel == null) return;
        shopPanel.SetActive(false);
        IsOpen = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void BuildItemList()
    {
        if (itemListParent == null || itemButtonPrefab == null) return;

        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);

        foreach (VendorItem item in items)
        {
            VendorItem captured = item;
            GameObject btn = Instantiate(itemButtonPrefab, itemListParent);

            ItemButtonUI buttonUI = btn.GetComponent<ItemButtonUI>();
            if (buttonUI != null)
            {
                if (buttonUI.nameText != null)
                    buttonUI.nameText.text = $"{item.itemName}  —  {item.cost}g";
                if (buttonUI.descriptionText != null)
                    buttonUI.descriptionText.text = item.description;
            }

            Button b = btn.GetComponentInChildren<Button>();
            Debug.Log($"Button found on {item.itemName}: {b != null}");
            b?.onClick.AddListener(() => TryBuy(captured));
        }
    }

    private void TryBuy(VendorItem item)
    {
        Debug.Log("TryBuy called for: " + item.itemName);
        if (GoldManager.Instance == null)
        {
            Debug.LogWarning("VendorUI: GoldManager not found in scene.");
            return;
        }
        if (PlayerStats.Instance == null)
        {
            Debug.LogWarning("VendorUI: PlayerStats not found in scene.");
            return;
        }

        if (!GoldManager.Instance.TrySpend(item.cost))
        {
            ShowFeedback("Not enough gold!");
            return;
        }

        PlayerStats.Instance.ModifyStat(item.statToIncrease, item.increaseAmount);
        if (!string.IsNullOrEmpty(item.statToDecrease))
            PlayerStats.Instance.ModifyStat(item.statToDecrease, -item.decreaseAmount);

        ShowFeedback($"Bought {item.itemName}!");
    }

    private void ShowFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }
}
