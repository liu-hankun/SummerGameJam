using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeTool : MonoBehaviour, IKitchenTool
{
    public void Interact(player player)
    {
        string[] options = { "芒果果肉", "椰奶", "葡萄果肉" };
        string chosen = options[Random.Range(0, options.Length)];
        KitchenSystem.Instance.AddIngredient(chosen);
        Debug.Log("从冰箱拿出：" + chosen);
    }
}

