using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardTool : MonoBehaviour, IKitchenTool
{
    public void Interact(player player)
    {
        if (KitchenSystem.Instance.HasIngredient("芒果果肉"))
        {
            KitchenSystem.Instance.RemoveIngredient("芒果果肉");
            KitchenSystem.Instance.AddIngredient("切好的芒果");
            Debug.Log("芒果被切好了！");
        }
        else
        {
            Debug.Log("你没有芒果果肉可以切！");
        }
    }
}