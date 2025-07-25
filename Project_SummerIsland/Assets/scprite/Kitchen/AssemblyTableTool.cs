using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AssemblyTableTool : MonoBehaviour, IKitchenTool
{
    public void Interact(player player)
    {
        KitchenSystem.Instance.CheckRecipe();
    }
}