using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    [Header("UI")]
    public TextMeshProUGUI inventoryText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddItem(string itemName)
    {
        if (!inventory.ContainsKey(itemName))
            inventory[itemName] = 0;

        inventory[itemName]++;
        UpdateUI();
    }

    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
    }

    void UpdateUI()
    {
        if (inventoryText == null) return;

        string content = "we have:\n";
        foreach (var pair in inventory)
        {
            content += $"{pair.Key} x {pair.Value}\n";
        }
        inventoryText.text = content;
    }
}

