using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家控制 + 交互键位：
/// E = 打开冰箱（仅 FridgeTool）
/// Q = 提交（优先 SubmitCounterTool；否则在组装台仅“放材料”）
/// Space = 组合材料（仅在组装台、且玩家没拿物品时）
/// </summary>
public class player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Held Item")]
    public Transform headPoint;
    public GameObject heldItem = null;

    [Header("Nearby Tools (auto)")]
    private FridgeTool nearbyFridge = null;
    private AssemblyTableTool nearbyAssembly = null;
    private SubmitCounterTool nearbySubmit = null;
    private IKitchenTool nearbyOther = null;

    public GameObject gunUI;
    public GameObject hintUI;
    // —— 输入侧同帧防抖 ——
    private int _lastEFrame = -1;
    private int _lastQFrame = -1;
    private int _lastSpaceFrame = -1;

    private Vector2 moveInput;

    /// <summary>是否手持物体</summary>
    public bool IsHoldingItem => heldItem != null;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

    if (animator == null) animator = GetComponentInChildren<Animator>();

    // 永久冻结 2D Z 旋转，防止任何物理扭矩让玩家转圈
    rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
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

        // ====== E：打开冰箱 ======
        if (Input.GetKeyDown(KeyCode.E) && _lastEFrame != Time.frameCount)
        {
            _lastEFrame = Time.frameCount;
            if (nearbyFridge != null)
            {
                nearbyFridge.Interact(this);
            }
            else
            {
                // 靠近冰箱才能开；不在冰箱旁就不做事
            }
        }

        // ====== Q：提交（优先提交台；否则在组装台“放材料”） ======
        if (Input.GetKeyDown(KeyCode.Q) && _lastQFrame != Time.frameCount)
        {
            _lastQFrame = Time.frameCount;

            if (nearbySubmit != null)
            {
                nearbySubmit.Interact(this);
            }
            else if (nearbyAssembly != null)
            {
                // 仅做“放材料”操作；不做组合
                if (IsHoldingItem)
                {
                    nearbyAssembly.AddIngredientFromPlayer(this);
                }
                else
                {
                    Debug.Log("[player] 你没有拿材料，按 Q 只用于放材料；按 Space 可尝试组合。");
                }
            }
            else if (nearbyOther != null)
            {
                // 如果你还有其它 IKitchenTool，希望 Q 触发它们
                nearbyOther.Interact(this);
            }
        }

        // ====== Space：组合材料（必须在组装台旁，且玩家手上为空） ======
        if (Input.GetKeyDown(KeyCode.Space) && _lastSpaceFrame != Time.frameCount)
        {
            _lastSpaceFrame = Time.frameCount;

            if (nearbyAssembly != null)
            {
                if (!IsHoldingItem)
                {
                    nearbyAssembly.Combine(this);
                }
                else
                {
                    Debug.Log("[player] 你手上有物品，无法组合。请先把材料放到组装台（按 Q）。");
                }
            }
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
    /// 玩家接收一个新物品（会把旧的销毁），实例化在 headPoint。
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
            heldItem.transform.SetParent(headPoint, worldPositionStays: true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        CacheTool(collision, enter: true);

        if (collision.CompareTag("enemysarea"))
        {
            if (gunUI != null) gunUI.SetActive(true);
        }

        if (collision.CompareTag("hint"))
        {
            hintUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;
        CacheTool(collision, enter:false);

        if (collision.CompareTag("enemysarea"))
        {
            if (gunUI != null) gunUI.SetActive(false);
        }

        if (collision.CompareTag("hint"))
        {
            hintUI.SetActive(false);
        }
    }

    private void CacheTool(Collider2D col, bool enter)
    {
        // 优先检查具体类型
        var f = col.GetComponent<FridgeTool>();
        if (f != null) { nearbyFridge = enter ? f : (nearbyFridge == f ? null : nearbyFridge); }

        var a = col.GetComponent<AssemblyTableTool>();
        if (a != null) { nearbyAssembly = enter ? a : (nearbyAssembly == a ? null : nearbyAssembly); }

        var s = col.GetComponent<SubmitCounterTool>();
        if (s != null) { nearbySubmit = enter ? s : (nearbySubmit == s ? null : nearbySubmit); }

        // 其余 IKitchenTool（不覆盖上面的具体优先级）
        var any = col.GetComponent<MonoBehaviour>() as IKitchenTool;
        if (any != null && any != f && any != a && any != s)
        {
            if (enter) nearbyOther = any;
            else if (nearbyOther == any) nearbyOther = null;
        }
    }
}

/// <summary>
/// 兼容旧脚本：允许以 player.IsHoldingItem() 的形式调用。
/// </summary>
public static class PlayerExtensions
{
    public static bool IsHoldingItem(this player p) => p != null && p.IsHoldingItem;
}

