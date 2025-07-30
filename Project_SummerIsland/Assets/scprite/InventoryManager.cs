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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化库存
            inventory["mango"] = 3;
            inventory["coconut"] = 2;
            inventory["grape"] = 2;
            inventory["ice"] = 2;

            UpdateUI();
        }
        else
        {
            Destroy(gameObject);
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
        if (uiText == null) return;

        uiText.text = "";
        foreach (var pair in inventory)
        {
            uiText.text += $"{pair.Key}: {pair.Value}\n";
        }
    }

    public bool HasItem(string name)
    {
        return inventory.ContainsKey(name) && inventory[name] > 0;
    }
}
