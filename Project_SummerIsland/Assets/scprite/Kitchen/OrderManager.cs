using UnityEngine;
using System.Collections;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public RecipeBook recipeBook;
    public string currentOrder = "";
    public int score = 0;

    public float orderTimeLimit = 60f;
    private float timer;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        PickNewOrder();
    }

    void Update()
    {
        if (currentOrder != "")
        {
            timer -= Time.deltaTime;
            UIOrderPanel.Instance?.UpdateTimerDisplay(timer);

            if (timer <= 0f)
            {
                Debug.Log("⏱️ 任务超时：" + currentOrder);
                PickNewOrder();
            }
        }
    }

    public void PickNewOrder()
    {
        if (recipeBook == null || recipeBook.recipes.Count == 0)
        {
            Debug.LogError("📛 RecipeBook 未绑定或为空！");
            return;
        }

        int index = Random.Range(0, recipeBook.recipes.Count);
        currentOrder = recipeBook.recipes[index].recipeName;
        timer = orderTimeLimit;

        Debug.Log("🎯 新任务：" + currentOrder);
        UIOrderPanel.Instance?.UpdateDisplay(currentOrder, score);
    }

    public void CompleteOrder(string recipeName)
    {
        if (recipeName == currentOrder)
        {
            score += 10;
            Debug.Log("✅ 完成任务：" + recipeName + "，当前得分：" + score);
            PickNewOrder();
        }
        else
        {
            Debug.Log("❌ 提交错误任务：" + recipeName + "，当前任务是：" + currentOrder);
        }
    }
}
