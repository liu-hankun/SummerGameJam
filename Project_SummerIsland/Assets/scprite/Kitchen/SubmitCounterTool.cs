using UnityEngine;

/// <summary>
/// 提交台（Submit Counter）：
/// - 玩家靠近并按 Q 交互；
/// - 若手上有成品并且名称与当前订单匹配：+100 分，销毁成品，刷新新订单；
/// - 若不匹配：销毁成品（丢弃），不给分；
/// - 若没拿物品：提示。
/// 说明：
/// - 分数走 ScoreManager（ScoreUI.cs）；同时把 OrderManager.score 同步为 ScoreManager 的当前分数，
///   以便 UIOrderPanel 的分数字段也能正确显示。
/// - 名称匹配基于字符串“归一化”（Trim + ToLowerInvariant）。
/// </summary>
public class SubmitCounterTool : MonoBehaviour, IKitchenTool
{
    [Header("Debounce")]
    [Tooltip("两次交互的最小间隔（秒）")]
    [SerializeField] private float interactCooldown = 0.2f;
    private float lastInteractTime = -1f;
    private int _lastInteractFrame = -1;

    public void Interact(player player)
    {
        if (player == null) return;

        // —— 同帧防抖 + 冷却 ——
        if (_lastInteractFrame == Time.frameCount) return;
        if (Time.time - lastInteractTime < interactCooldown) return;
        _lastInteractFrame = Time.frameCount;
        lastInteractTime   = Time.time;

        if (player.heldItem == null)
        {
            Debug.Log("[SubmitCounter] 玩家没有拿物品，无法提交。");
            return;
        }

        string dishInHand = ExtractName(player.heldItem);
        string currentOrder = OrderManager.Instance != null ? OrderManager.Instance.currentOrder : null;

        if (string.IsNullOrEmpty(currentOrder))
        {
            Debug.LogWarning("[SubmitCounter] 当前没有订单。物品将被丢弃。");
            DestroyHeld(player);
            return;
        }

        if (Normalize(dishInHand) == Normalize(currentOrder))
        {
            // ✅ 符合要求：加 100 分，销毁成品，刷新新订单
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(100);
            }

            if (OrderManager.Instance != null)
            {
                // 让 UIOrderPanel 的分数与 ScoreManager 对齐
                if (ScoreManager.Instance != null)
                    OrderManager.Instance.score = ScoreManager.Instance.GetScore();

                OrderManager.Instance.PickNewOrder();
            }

            Debug.Log($"[SubmitCounter] 提交成功：{dishInHand}，+100 分");
            DestroyHeld(player); // 交付后移除
        }
        else
        {
            // ❌ 不符合：丢弃物品
            Debug.Log($"[SubmitCounter] 提交失败：手上是 {dishInHand}，当前订单是 {currentOrder}，物品已丢弃。");
            DestroyHeld(player);
        }
    }

    private static void DestroyHeld(player player)
    {
        if (player.heldItem != null)
        {
            Object.Destroy(player.heldItem);
            player.heldItem = null;
        }
    }

    private static string ExtractName(GameObject go)
    {
        if (go == null) return "";
        // 优先读自定义组件（如果你给成品 prefab 挂了 FoodName，建议使用）
        var fn = go.GetComponent<FoodName>();
        if (fn != null && !string.IsNullOrEmpty(fn.recipeName))
            return fn.recipeName;

        // 退化：使用 GameObject 的名字
        return go.name.Replace("(Clone)", string.Empty).Trim();
    }

    private static string Normalize(string s)
    {
        return string.IsNullOrEmpty(s) ? "" : s.Trim().ToLowerInvariant();
    }
}

/// <summary>
/// 可选的小组件：给成品预制体标注其配方名，便于精确匹配。
/// （你可以不挂这个脚本，SubmitCounter 也会用 GameObject.name 做匹配。）
/// </summary>
public class FoodName : MonoBehaviour
{
    public string recipeName;
}
