using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FridgeUIManager : MonoBehaviour
{
    public GameObject fridgePanel;

    public Button mangoButton;
    public Button coconutButton;
    public Button grapeButton;
    public Button iceButton;

    public GameObject mangoPrefab;
    public GameObject coconutPrefab;
    public GameObject grapePrefab;
    public GameObject icePrefab;

    public TextMeshProUGUI inventoryText;

    private void Awake()
    {
        // 绑定按钮点击；仅负责“拿到手上”，不直接改 KitchenSystem（避免重复加料）
        mangoButton.onClick.AddListener(() => SafeSelectItem("mango"));
        coconutButton.onClick.AddListener(() => SafeSelectItem("coconut"));
        grapeButton.onClick.AddListener(() => SafeSelectItem("grape"));
        iceButton.onClick.AddListener(() => SafeSelectItem("ice"));
    }

    private void SafeSelectItem(string itemName)
    {
        try
        {
            Debug.Log($"🟡 尝试点击按钮：{itemName}");
            SelectItem(itemName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ SelectItem('{itemName}') 出错：{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void OpenPanel()
    {
        fridgePanel.SetActive(true);

        Dictionary<string, int> inv = InventoryManager.Instance?.inventory;
        if (inv == null)
        {
            Debug.LogError("❌ InventoryManager.Instance 或 inventory 是 null！");
            return;
        }

        mangoButton.interactable = GetSafeCount(inv, "mango") > 0;
        coconutButton.interactable = GetSafeCount(inv, "coconut") > 0;
        grapeButton.interactable = GetSafeCount(inv, "grape") > 0;
        iceButton.interactable = GetSafeCount(inv, "ice") > 0;

        RefreshInventoryDisplay();
    }

    public void SelectItem(string itemName)
    {
        itemName = itemName.ToLower();

        var p = FindObjectOfType<player>();
        if (p == null)
        {
            Debug.LogError("❌ 未找到玩家对象！");
            ClosePanel();
            return;
        }

        if (p.headPoint == null)
        {
            Debug.LogError("❌ 玩家未绑定 headPoint，请在 Inspector 中设置！");
            ClosePanel();
            return;
        }

        if (p.IsHoldingItem())
        {
            Debug.Log("⚠️ 玩家已持有物品，不能再拿！");
            ClosePanel();
            return;
        }

        var inv = InventoryManager.Instance?.inventory;
        if (inv == null)
        {
            Debug.LogError("❌ InventoryManager 或 inventory 为 null！");
            ClosePanel();
            return;
        }

        if (!InventoryManager.Instance.HasItem(itemName))
        {
            Debug.LogWarning($"⚠️ 库存中没有 {itemName}！");
            ClosePanel();
            return;
        }

        GameObject prefab = GetPrefabByName(itemName);
        if (prefab == null)
        {
            Debug.LogError($"❌ 未找到预制体：{itemName}！请确认在 Inspector 中绑定了 prefab！");
            ClosePanel();
            return;
        }

        // 一切正常：扣库存 → 生成 → 给玩家手上
        InventoryManager.Instance.UseItem(itemName);
        p.ReceiveItem(prefab);

        // 关键修复：不要在冰箱阶段往 KitchenSystem 里加材料
        // 由 AssemblyTableTool 在玩家“放置”时再 AddIngredient()

        Debug.Log($"✅ 生成 {itemName} 到玩家手上！");
        ClosePanel();
    }

    public void ClosePanel()
    {
        fridgePanel.SetActive(false);

        mangoButton.interactable = false;
        coconutButton.interactable = false;
        grapeButton.interactable = false;
        iceButton.interactable = false;
    }

    private void RefreshInventoryDisplay()
    {
        Dictionary<string, int> inv = InventoryManager.Instance?.inventory;
        if (inv == null || inventoryText == null)
        {
            Debug.LogWarning("⚠️ UI 或 InventoryManager 未初始化！");
            return;
        }

        inventoryText.text = "Your Food:\n";
        inventoryText.text += $"Mango: {GetSafeCount(inv, "mango")}\n";
        inventoryText.text += $"Coconut: {GetSafeCount(inv, "coconut")}\n";
        inventoryText.text += $"Grape: {GetSafeCount(inv, "grape")}\n";
        inventoryText.text += $"Ice: {GetSafeCount(inv, "ice")}\n";
    }

    private int GetSafeCount(Dictionary<string, int> inv, string key)
    {
        return inv.ContainsKey(key) ? inv[key] : 0;
    }

    private GameObject GetPrefabByName(string itemName)
    {
        switch (itemName)
        {
            case "mango": return mangoPrefab;
            case "coconut": return coconutPrefab;
            case "grape": return grapePrefab;
            case "ice": return icePrefab;
            default: return null;
        }
    }
}
