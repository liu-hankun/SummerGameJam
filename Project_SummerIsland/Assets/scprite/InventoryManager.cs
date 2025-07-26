using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public TextMeshProUGUI uiText;

    private void Awake()
    {
        Instance = this;
        inventory["mango"] = 0;
        inventory["coconut"] = 0;
        inventory["grape"] = 0;
        inventory["Ice"] = 0;
        UpdateUI();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddItem(string name)
    {
        if (!inventory.ContainsKey(name)) inventory[name] = 0;
        inventory[name]++;
        UpdateUI();
    }

    public bool UseItem(string name)
    {
        if (inventory.ContainsKey(name) && inventory[name] > 0)
        {
            inventory[name]--;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void UpdateUI()
    {
        uiText.text = " ";
        foreach (var pair in inventory)
        {
            uiText.text += $"{pair.Key}: {pair.Value}\n";
        }
    }

    public bool HasItem(string name) => inventory.ContainsKey(name) && inventory[name] > 0;
}
