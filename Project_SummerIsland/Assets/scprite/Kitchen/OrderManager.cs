using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public RecipeBook recipeBook; // 任务候选菜单
    public string currentOrder = "";
    public int score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        PickNewOrder();
    }

    public void PickNewOrder()
    {
        if (recipeBook.recipes.Count == 0)
        {
            Debug.LogWarning("配方为空，无法生成任务！");
            return;
        }

        int index = Random.Range(0, recipeBook.recipes.Count);
        currentOrder = recipeBook.recipes[index].recipeName;
        UIOrderPanel.Instance?.UpdateDisplay(currentOrder, score);
        Debug.Log("新任务：制作 " + currentOrder);
    }

    public void CompleteOrder(string recipeName)
    {
        if (recipeName == currentOrder)
        {
            score += 10;
            Debug.Log("任务完成！+10分，总分：" + score);
            PickNewOrder();
        }
        else
        {
            Debug.Log("完成了 " + recipeName + "，但不是当前任务：" + currentOrder);
        }

        UIOrderPanel.Instance?.UpdateDisplay(currentOrder, score);
    }
}
