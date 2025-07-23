using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoardTool : MonoBehaviour, IKitchenTool
{
    public void Interact(player player)
    {
        bool didCut = false;

        if (KitchenSystem.Instance.HasIngredient("芒果果肉"))
        {
            KitchenSystem.Instance.RemoveIngredient("芒果果肉");
            KitchenSystem.Instance.AddIngredient("切好的芒果");
            Debug.Log("你切好了芒果！");
            didCut = true;
        }

        if (KitchenSystem.Instance.HasIngredient("葡萄果肉"))
        {
            KitchenSystem.Instance.RemoveIngredient("葡萄果肉");
            KitchenSystem.Instance.AddIngredient("切好的葡萄");
            Debug.Log("你切好了葡萄！");
            didCut = true;
        }

        if (!didCut)
        {
            Debug.Log("你没有可切的水果！");
        }
    }
}