using System.Collections.Generic;
using UnityEngine;

public class KitchenSystem : MonoBehaviour
{
    public static KitchenSystem Instance;
    private List<string> currentIngredients = new List<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddIngredient(string name)
    {
        currentIngredients.Add(name);
        Debug.Log("添加食材：" + name);
    }

    public bool HasIngredient(string name)
    {
        return currentIngredients.Contains(name);
    }

    public void RemoveIngredient(string name)
    {
        currentIngredients.Remove(name);
        Debug.Log("移除食材：" + name);
    }

    public void PrintIngredients()
    {
        Debug.Log("当前食材：" + string.Join(", ", currentIngredients));
    }
}