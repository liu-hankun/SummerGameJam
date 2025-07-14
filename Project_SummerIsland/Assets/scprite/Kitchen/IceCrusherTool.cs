using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceCrusherTool : MonoBehaviour, IKitchenTool
{
    public void Interact(player player)
    {
        if (KitchenSystem.Instance.HasIngredient("冰块"))
        {
            KitchenSystem.Instance.RemoveIngredient("冰块");
            KitchenSystem.Instance.AddIngredient("刨冰");
            Debug.Log("冰块变成了刨冰！");
        }
        else
        {
            Debug.Log("你没有冰块可以打碎！");
        }
    }
}