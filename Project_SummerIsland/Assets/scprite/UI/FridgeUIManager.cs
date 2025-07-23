using UnityEngine;
using UnityEngine.UI;

public class FridgeUIManager : MonoBehaviour
{
    public GameObject fridgePanel;
    public Button mangoButton;
    public Button coconutButton;
    public Button grapeButton;

    // 打开面板时由 FridgeTool 调用
    public void OpenPanel()
    {
        fridgePanel.SetActive(true);

        // 激活按钮
        mangoButton.interactable = true;
        coconutButton.interactable = true;
        grapeButton.interactable = true;
    }

    // 玩家点击按钮选食材时调用
    public void SelectItem(string itemName)
    {
        KitchenSystem.Instance.AddIngredient(itemName);
        Debug.Log("你从冰箱拿出：" + itemName);

        ClosePanel(); // 自动关闭
    }

    // 点击“关闭”按钮或选择后关闭
    public void ClosePanel()
    {
        fridgePanel.SetActive(false);

        // 禁用按钮
        mangoButton.interactable = false;
        coconutButton.interactable = false;
        grapeButton.interactable = false;
    }
}