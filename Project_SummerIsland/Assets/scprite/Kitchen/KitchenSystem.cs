using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KitchenSystem : MonoBehaviour
{
    public static KitchenSystem Instance;
    public RecipeBook recipeBook;
    private List<string> currentIngredients = new List<string>();

    // —— New: same-frame duplicate guard ——
    private int    _lastAddFrame = -1;
    private string _lastAddName  = null;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddIngredient(string name)
    {
        // Normalize name to avoid duplicates like "Mango", "mango ", etc.
        string n = NormalizeName(name);

        // Same-frame same-name guard
        if (_lastAddFrame == Time.frameCount && _lastAddName == n)
        {
            Debug.Log($"[KitchenSystem] same-frame duplicate blocked: {n}");
            return;
        }
        _lastAddFrame = Time.frameCount;
        _lastAddName  = n;

        currentIngredients.Add(n);
        Debug.Log("添加食材：" + n);
        // Optional: enable to trace who called AddIngredient
        // Debug.Log($"[KitchenSystem] AddIngredient caller:\n{System.Environment.StackTrace}");
    }

    public bool HasIngredient(string name)
    {
        return currentIngredients.Contains(NormalizeName(name));
    }

    public void RemoveIngredient(string name)
    {
        string n = NormalizeName(name);
        currentIngredients.Remove(n);
        Debug.Log("移除食材：" + n);
    }

    public void PrintIngredients()
    {
        Debug.Log("当前食材：" + string.Join(", ", currentIngredients));
    }

    public void CheckRecipe()
    {
        foreach (var recipe in recipeBook.recipes)
        {
            if (MatchRecipe(recipe.ingredients))
            {
                Debug.Log("你制作了料理：" + recipe.recipeName);
                OrderManager.Instance?.CompleteOrder(recipe.recipeName); // ✅ 任务判断 + 加分
                currentIngredients.Clear();
                return;
            }
        }

        Debug.Log("组合失败，没有匹配的料理！");
    }

    private bool MatchRecipe(List<string> required)
    {
        List<string> current = new List<string>(currentIngredients);
        List<string> check = new List<string>(required);

        current = current.ConvertAll(NormalizeName);
        check = check.ConvertAll(NormalizeName);

        current.Sort();
        check.Sort();

        // 打印以便调试
        Debug.Log("🧪 当前材料：" + string.Join(", ", current));
        Debug.Log("🧪 目标配方：" + string.Join(", ", check));

        return current.SequenceEqual(check);
    }

    public int CurrentIngredientCount()
    {
        return currentIngredients.Count;
    }

    private static string NormalizeName(string s)
    {
        return string.IsNullOrEmpty(s) ? "" : s.Trim().ToLowerInvariant();
    }
}
