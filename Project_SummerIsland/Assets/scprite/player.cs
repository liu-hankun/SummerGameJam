using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制 + 统一交互逻辑。
/// 修复点：
/// 1) 同帧防抖：一次按键只会触发一次 Interact()；
/// 2) 交互目标唯一：优先和组装台交互，其次其他工具；
/// 3) 提供 IsHoldingItem 属性；另外提供扩展方法 IsHoldingItem() 兼容旧代码。
/// </summary>
public class player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Held Item")]
    public Transform headPoint;       // 手持物体的挂点
    public GameObject heldItem = null;

    [Header("Nearby Tools")]
    private IKitchenTool nearbyTool = null;            // 其它工具（如冰箱、切菜板等）
    private IKitchenTool nearbyAssemblyTool = null;    // 组装台（优先交互）

    // —— 输入侧同帧防抖 ——（防止同一帧多次触发）
    private int _lastInteractFrame = -1;

    private Vector2 moveInput;

    /// <summary>
    /// 是否正在手持物体（给 Fridge/UI 等脚本调用）
    /// </summary>
    public bool IsHoldingItem => heldItem != null;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // ====== 移动输入 ======
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        if (animator != null)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical",   moveInput.y);
            animator.SetFloat("Speed",      moveInput.sqrMagnitude);
        }

        // ====== 交互输入（按 Q）—— 同帧只触发一次 ======
        // 如果你想让 Space 也能交互，把下面条件改成：
        // if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AttemptInteractOncePerFrame();
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 在同一帧内只允许一次 Interact 调用（无论你按了几个键或被多次触发）。
    /// </summary>
    private void AttemptInteractOncePerFrame()
    {
        if (_lastInteractFrame == Time.frameCount) return; // 同帧重复拦截
        _lastInteractFrame = Time.frameCount;

        // 只选择一个交互目标：优先组装台
        IKitchenTool target = nearbyAssemblyTool ?? nearbyTool;
        if (target != null)
        {
            target.Interact(this);
        }
    }

    /// <summary>
    /// 玩家接收/切换手上物体（会销毁旧的）。
    /// </summary>
    public void ReceiveItem(GameObject itemPrefab)
    {
        if (heldItem != null)
        {
            Destroy(heldItem);
            heldItem = null;
        }

        if (itemPrefab != null && headPoint != null)
        {
            heldItem = Instantiate(itemPrefab, headPoint.position, Quaternion.identity);
            // 建议把手持物体作为headPoint的子物体，方便定位
            heldItem.transform.SetParent(headPoint, worldPositionStays: true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (TryGetKitchenTool(collision, out var tool))
        {
            // 若是组装台则记到 nearbyAssemblyTool，否则记到 nearbyTool
            if (tool is AssemblyTableTool)
                nearbyAssemblyTool = tool;
            else
                nearbyTool = tool;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (TryGetKitchenTool(collision, out var tool))
        {
            if (nearbyAssemblyTool == tool) nearbyAssemblyTool = null;
            if (nearbyTool == tool) nearbyTool = null;
        }
    }

    /// <summary>
    /// 兼容不同 Unity 版本：从碰撞体上寻找任何实现了 IKitchenTool 的组件。
    /// </summary>
    private bool TryGetKitchenTool(Collider2D col, out IKitchenTool tool)
    {
        tool = null;
        if (col == null) return false;

        // 直接拿接口（新版本 Unity 对接口 TryGetComponent 兼容得更好）
        // 如果你的 Unity 版本支持，可以用：col.TryGetComponent<IKitchenTool>(out tool)
        // 这里使用更兼容的遍历方式：
        var behaviours = col.GetComponents<MonoBehaviour>();
        for (int i = 0; i < behaviours.Length; i++)
        {
            var mb = behaviours[i];
            if (mb is IKitchenTool t)
            {
                tool = t;
                return true;
            }
        }

        // 父层再找一遍，防止触发器在子物体上、工具脚本在父物体上
        var parentBehaviours = col.GetComponentsInParent<MonoBehaviour>();
        for (int i = 0; i < parentBehaviours.Length; i++)
        {
            var mb = parentBehaviours[i];
            if (mb is IKitchenTool t)
            {
                tool = t;
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// 兼容旧脚本的扩展方法：允许写成 player.IsHoldingItem()。
/// </summary>
public static class PlayerExtensions
{
    public static bool IsHoldingItem(this player p)
    {
        return p != null && p.IsHoldingItem;
    }
}
