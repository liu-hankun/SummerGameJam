using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FoodPrefab
{
    public string recipeName;
    public GameObject prefab;
}

public class KitchenSystem : MonoBehaviour
{
    public static KitchenSystem Instance;
    public RecipeBook recipeBook;

    [Header("Ingredients (runtime)")]
    [SerializeField] private List<string> currentIngredients = new List<string>();

    [Header("Food Prefabs (drag from Assets/food)")]
    public List<FoodPrefab> foodPrefabs = new List<FoodPrefab>();

    // —— Same-frame duplicate guard ——
    private int    _lastAddFrame = -1;
    private string _lastAddName  = null;

    // —— Lookup built at runtime ——
    private Dictionary<string, GameObject> _foodLookup = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // build lookup from list
        _foodLookup.Clear();
        foreach (var fp in foodPrefabs)
        {
            if (fp == null || string.IsNullOrEmpty(fp.recipeName) || fp.prefab == null) continue;
            var key = NormalizeName(fp.recipeName);
            _foodLookup[key] = fp.prefab;
        }
    }

    public void AddIngredient(string name)
    {
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

    /// <summary>
    /// 兼容旧逻辑：仅打印成功/失败。不实例化食物，由工具侧（组装台）决定何时给玩家。
    /// </summary>
    public void CheckRecipe()
    {
        if (TryCraft(out var dishName))
        {
            Debug.Log("你制作了料理：" + dishName);
        }
        else
        {
            Debug.Log("组合失败，没有匹配的料理！");
        }
    }

    /// <summary>
    /// 判定是否匹配某个配方；若成功，清空材料、完成订单，并返回菜名。
    /// </summary>
    public bool TryCraft(out string craftedRecipeName)
    {
        craftedRecipeName = null;

        if (recipeBook == null || recipeBook.recipes == null || recipeBook.recipes.Count == 0)
        {
            Debug.LogWarning("[KitchenSystem] RecipeBook 未配置或为空。");
            return false;
        }

        foreach (var recipe in recipeBook.recipes)
        {
            if (recipe == null) continue;
            if (MatchRecipe(recipe.ingredients))
            {
                craftedRecipeName = recipe.recipeName;
                Debug.Log("你制作了料理：" + craftedRecipeName);

                // 任务/计分（如有）
                if (OrderManager.Instance != null)
                {
                    OrderManager.Instance.CompleteOrder(craftedRecipeName);
                }

                currentIngredients.Clear();
                return true;
            }
        }

        return false;
    }

    private bool MatchRecipe(List<string> required)
    {
        // clone lists
        List<string> current = new List<string>(currentIngredients);
        List<string> check = new List<string>(required ?? new List<string>());

        // normalize + sort
        current = current.ConvertAll(NormalizeName);
        check   = check.ConvertAll(NormalizeName);

        current.Sort();
        check.Sort();

        // 调试打印
        Debug.Log("🧪 当前材料：" + string.Join(", ", current));
        Debug.Log("🧪 目标配方：" + string.Join(", ", check));

        return current.SequenceEqual(check);
    }

    public int CurrentIngredientCount()
    {
        return currentIngredients.Count;
    }

    /// <summary>
    /// 由工具侧（组装台）来取 prefab 并实例化在玩家身上。
    /// </summary>
    public GameObject GetFoodPrefab(string recipeName)
    {
        if (string.IsNullOrEmpty(recipeName)) return null;
        var key = NormalizeName(recipeName);
        _foodLookup.TryGetValue(key, out var prefab);
        return prefab;
    }

    private static string NormalizeName(string s)
    {
        return string.IsNullOrEmpty(s) ? "" : s.Trim().ToLowerInvariant();
    }
}
