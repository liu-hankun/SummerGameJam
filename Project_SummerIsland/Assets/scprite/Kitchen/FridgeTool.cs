using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FridgeTool : MonoBehaviour, IKitchenTool
{
    public FridgeUIManager uiManager;

    public void Interact(player player)
    {
        if (player != null && player.IsHoldingItem())
        {
            Debug.Log("你已经拿着食材，不能再打开冰箱！");
            return;
        }

        if (uiManager != null)
        {
            uiManager.OpenPanel();
        }
        else
        {
            Debug.LogWarning("FridgeTool 未连接 FridgeUIManager！");
        }
    }
}
