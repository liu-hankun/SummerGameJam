using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // 使用小写规范名作为键
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public TextMeshProUGUI uiText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 如未初始化，给一点起始值；没有的话也没关系
            EnsureKey("mango", 1);
            EnsureKey("coconut", 1);
            EnsureKey("grape", 1);
            EnsureKey("ice", 1);

            UpdateUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static string Normalize(string name)
    {
        return string.IsNullOrEmpty(name) ? "" : name.Trim().ToLowerInvariant();
    }

    private void EnsureKey(string key, int init = 0)
    {
        key = Normalize(key);
        if (!inventory.ContainsKey(key))
            inventory[key] = init;
    }

    /// <summary>
    /// 增加物品：会自动把名称规范化（小写、去空格）；返回增加后的数量。
    /// </summary>
    public int AddItem(string name)
    {
        string key = Normalize(name);
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("[Inventory] AddItem: name is empty");
            return 0;
        }

        if (!inventory.ContainsKey(key)) inventory[key] = 0;
        inventory[key]++;
        Debug.Log($"[Inventory] +1 {key} => {inventory[key]}");

        UpdateUI();
        return inventory[key];
    }

    /// <summary>
    /// 使用物品（-1）；成功返回 true。
    /// </summary>
    public bool UseItem(string name)
    {
        string key = Normalize(name);
        if (string.IsNullOrEmpty(key)) return false;

        if (inventory.ContainsKey(key) && inventory[key] > 0)
        {
            inventory[key]--;
            Debug.Log($"[Inventory] -1 {key} => {inventory[key]}");
            UpdateUI();
            return true;
        }
        return false;
    }

    public int GetCount(string name)
    {
        string key = Normalize(name);
        if (inventory.TryGetValue(key, out var v)) return v;
        return 0;
    }

    public bool HasItem(string name)
    {
        return GetCount(name) > 0;
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
}
