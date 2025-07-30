using UnityEngine;

/// <summary>
/// 组装台：把玩家手里的材料放入 KitchenSystem；如果没有拿物品则尝试合成。
/// 强化修复：
/// - 全局同帧保险丝（static）：同一帧内任何 AssemblyTableTool 只处理一次。
/// - 实例内同帧保险丝 + 冷却：防止抖动和重复触发。
/// - 详细日志：便于定位是否存在多个工具脚本实例被同时触发。
/// </summary>
public class AssemblyTableTool : MonoBehaviour, IKitchenTool
{
    [Header("Debounce")]
    [Tooltip("两次交互的最小间隔（秒）")]
    [SerializeField] private float interactCooldown = 0.2f;

    private float lastInteractTime = -1f;
    private int _lastInteractFrame = -1;

    // —— 全局同帧保险丝（跨所有实例共享） ——
    private static int s_lastGlobalInteractFrame = -1;

    private KitchenSystem kitchenSystem;

    private void Awake()
    {
        kitchenSystem = FindObjectOfType<KitchenSystem>();
        if (kitchenSystem == null)
        {
            Debug.LogError("❌ 无法找到 KitchenSystem！");
        }
    }

    public void Interact(player player)
    {
        if (player == null) return;

        // 调试日志：看一眼是否被多实例同帧触发
        // 你可以在运行时查看 Console：相同帧出现多个不同 InstanceID 的日志，说明有多个脚本实例在响应一次按键。
        Debug.Log($"[AssemblyTableTool] Interact call - frame={Time.frameCount}, inst={GetInstanceID()}, hasHeld={(player.heldItem != null)}");

        // —— 全局防重复：同一帧任何实例只处理一次 ——
        if (s_lastGlobalInteractFrame == Time.frameCount) return;

        // —— 实例内防重复：同一帧当前实例只处理一次 ——
        if (_lastInteractFrame == Time.frameCount) return;

        // —— 冷却：保证不是在极短时间内的重复触发 ——
        if (Time.time - lastInteractTime < interactCooldown) return;

        s_lastGlobalInteractFrame = Time.frameCount;
        _lastInteractFrame = Time.frameCount;
        lastInteractTime   = Time.time;

        // 1) 如果玩家手里有物体：放入 KitchenSystem
        if (player.heldItem != null)
        {
            string itemName = ExtractItemName(player.heldItem);
            if (string.IsNullOrEmpty(itemName))
            {
                Debug.LogWarning("⚠️ 未能识别手中物体名称，已跳过。");
                return;
            }

            if (kitchenSystem != null)
            {
                Debug.Log($"[AssemblyTableTool] AddIngredient: {itemName}");
                kitchenSystem.AddIngredient(itemName);
            }

            Object.Destroy(player.heldItem);
            player.heldItem = null;
        }
        else
        {
            // 2) 如果没拿物体：尝试组合（由 KitchenSystem 内部判定是否满足配方）
            if (kitchenSystem != null)
            {
                Debug.Log("[AssemblyTableTool] CheckRecipe()");
                kitchenSystem.CheckRecipe();
            }
        }
    }

    /// <summary>
    /// 直接使用物体名（去掉 "(Clone)"；如果在挂点下还有子物体，优先使用根物体名）。
    /// </summary>
    private string ExtractItemName(GameObject go)
    {
        if (go == null) return null;

        string raw = go.name;
        if (string.IsNullOrEmpty(raw)) return null;

        return raw.Replace("(Clone)", string.Empty).Trim();
    }
}
