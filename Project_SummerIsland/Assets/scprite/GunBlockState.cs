using UnityEngine;

/// <summary>
/// 限制枪械开火的状态计数器：
/// - blockCount > 0 时禁用玩家身上的所有 BulletShooter 组件；
/// - 支持多个区域叠加。
/// - 将该组件挂在玩家的根对象（含 player 组件）上。
/// </summary>
public class GunBlockState : MonoBehaviour
{
    [SerializeField] private int blockCount = 0;

    /// <summary>增加一层禁用（如进入一个 KitchenAreaNoGun）。</summary>
    public void Increment()
    {
        blockCount++;
        Apply();
    }

    /// <summary>减少一层禁用（如离开一个 KitchenAreaNoGun）。</summary>
    public void Decrement()
    {
        blockCount = Mathf.Max(0, blockCount - 1);
        Apply();
    }

    private void Apply()
    {
        bool disable = blockCount > 0;
        var shooters = GetComponentsInChildren<BulletShooter>(true);
        foreach (var s in shooters)
        {
            if (s != null) s.enabled = !disable;
        }
    }
}
