using UnityEngine;

/// <summary>
/// 组装台：
/// - AddIngredientFromPlayer(player)：把玩家手里的材料放入 KitchenSystem；
/// - Combine(player)：尝试组合（玩家手上需为空），成功则给玩家成品；
/// 仍保留原 Interact(player) 兼容旧逻辑（有物放料；无物合成）。
/// </summary>
public class AssemblyTableTool : MonoBehaviour, IKitchenTool
{
    [Header("Debounce")]
    [Tooltip("两次交互的最小间隔（秒）")]
    [SerializeField] private float interactCooldown = 0.2f;

    private float lastInteractTime = -1f;
    private int _lastInteractFrame = -1;

    // 全局同帧保险丝（跨所有实例共享）
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

        // 全局/实例防抖 + 冷却
        if (s_lastGlobalInteractFrame == Time.frameCount) return;
        if (_lastInteractFrame == Time.frameCount) return;
        if (Time.time - lastInteractTime < interactCooldown) return;
        s_lastGlobalInteractFrame = Time.frameCount;
        _lastInteractFrame = Time.frameCount;
        lastInteractTime   = Time.time;

        if (player.heldItem != null) AddIngredientFromPlayer(player);
        else Combine(player);
    }

    /// <summary>仅做“放材料”。</summary>
    public void AddIngredientFromPlayer(player player)
    {
        if (player == null) return;

        if (player.heldItem == null)
        {
            Debug.Log("[AssemblyTable] 你没有拿材料，无法放入。");
            return;
        }

        string itemName = ExtractItemName(player.heldItem);
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("[AssemblyTable] 未能识别手中物体名称，已跳过。");
            return;
        }

        if (kitchenSystem != null)
        {
            Debug.Log($"[AssemblyTable] AddIngredient: {itemName}");
            kitchenSystem.AddIngredient(itemName);
        }

        Object.Destroy(player.heldItem);
        player.heldItem = null;
    }

    /// <summary>仅做“组合材料”。</summary>
    public void Combine(player player)
    {
        if (player == null) return;

        if (player.heldItem != null)
        {
            Debug.Log("[AssemblyTable] 你手上有物品，无法组合。");
            return;
        }

        if (kitchenSystem == null) return;

        if (kitchenSystem.TryCraft(out var dish))
        {
            var prefab = kitchenSystem.GetFoodPrefab(dish);
            if (prefab != null)
            {
                player.ReceiveItem(prefab); // 实例化到 headPoint 并挂在玩家身上

                if (player.heldItem != null)
                {
                    // 将实例 GameObject 名设为配方名（便于提交台匹配）
                    player.heldItem.name = dish;

                    var fn = player.heldItem.GetComponent<FoodName>();
                    if (fn != null) fn.recipeName = dish;
                }
                Debug.Log($"[AssemblyTable] Crafted and given to player: {dish}");
            }
            else
            {
                Debug.LogWarning($"[AssemblyTable] 找不到成品预制体：{dish}。请在 KitchenSystem 的 Food Prefabs 列表里配置，或放到 Resources/food 下。");
            }
        }
        else
        {
            Debug.Log("[AssemblyTable] 没有匹配的配方。");
        }
    }

    private string ExtractItemName(GameObject go)
    {
        if (go == null) return null;
        string raw = go.name;
        if (string.IsNullOrEmpty(raw)) return null;
        return raw.Replace("(Clone)", string.Empty).Trim();
    }
}
