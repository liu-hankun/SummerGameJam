using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KitchenSystem : MonoBehaviour
{
    public static KitchenSystem Instance;
    public RecipeBook recipeBook;
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
   
    public void CheckRecipe()
    {
        foreach (var recipe in recipeBook.recipes)
        {
            if (MatchRecipe(recipe.ingredients))
            {
                Debug.Log("你制作了料理：" + recipe.recipeName);
                currentIngredients.Clear();
                return;
            }
        }

        Debug.Log("组合失败，没有匹配的料理！");
    }

    private bool MatchRecipe(List<string> required)
    {
        List<string> current = new List<string>(currentIngredients);
        current.Sort();
        List<string> check = new List<string>(required);
        check.Sort();

        return current.SequenceEqual(check);
    }
}