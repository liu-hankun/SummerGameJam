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

    public GameObject mangoPrefab;
    public GameObject coconutPrefab;
    public GameObject grapePrefab;

    public TextMeshProUGUI inventoryText;

    public void OpenPanel()
    {
        fridgePanel.SetActive(true);

        mangoButton.interactable = true;
        coconutButton.interactable = true;
        grapeButton.interactable = true;

        RefreshInventoryDisplay();
    }

    public void SelectItem(string itemName)
    {
        player player = FindObjectOfType<player>();
        if (player == null || player.IsHoldingItem())
        {
            Debug.Log("玩家不存在或手上已有物品！");
            ClosePanel();
            return;
        }

        GameObject prefab = null;

        switch (itemName)
        {
            case "Mango":
                prefab = mangoPrefab;
                break;
            case "Coconut":
                prefab = coconutPrefab;
                break;
            case "Grape":
                prefab = grapePrefab;
                break;
        }

        if (prefab != null)
        {
            player.ReceiveItem(prefab); // 玩家手持预制体
            KitchenSystem.Instance.AddIngredient(itemName); // 更新系统数据
            Debug.Log("从冰箱拿出：" + itemName);
        }

        ClosePanel();
    }

    public void ClosePanel()
    {
        fridgePanel.SetActive(false);

        mangoButton.interactable = false;
        coconutButton.interactable = false;
        grapeButton.interactable = false;
    }

    private void RefreshInventoryDisplay()
    {
        if (InventoryManager.Instance == null || inventoryText == null)
        {
            Debug.LogWarning("InventoryManager 或 UI 尚未初始化！");
            return;
        }

        Dictionary<string, int> inv = InventoryManager.Instance.inventory;

        inventoryText.text = $"Your Food:\n";
        inventoryText.text += $"Mango:{GetSafeCount(inv, "mango")}\n";
        inventoryText.text += $"Cocount:{GetSafeCount(inv, "coconut")}\n";
        inventoryText.text += $"Grape:{GetSafeCount(inv, "grape")}\n";
        inventoryText.text += $"Ice:{GetSafeCount(inv, "Ice")}\n";
    }

    private int GetSafeCount(Dictionary<string, int> inv, string key)
    {
        return inv.ContainsKey(key) ? inv[key] : 0;
    }
}
