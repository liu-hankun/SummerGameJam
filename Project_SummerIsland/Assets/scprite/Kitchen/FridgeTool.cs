using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FridgeTool : MonoBehaviour, IKitchenTool
{
    public FridgeUIManager uiManager;

    public void Interact(player player)
    {
        if (uiManager != null)
        {
            uiManager.OpenPanel();
        }
    }
}