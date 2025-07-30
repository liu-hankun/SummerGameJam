using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    // 右键增加“ice”库存（名称统一使用小写规范名以匹配 InventoryManager）
    public string itemName = "ice";
    public float interactRadius = 2f;
    public GameObject dropPrefab;  

    private Transform player;
    private bool canInteract = true;

    void Start()
    {
        var playerGo = GameObject.FindGameObjectWithTag("Player");
        player = playerGo != null ? playerGo.transform : null;
        if (player == null)
        {
            Debug.LogWarning("[IceBlock] 未找到带有 Player 标签的对象。");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 右键（Mouse1）且距离足够近
        if (distance <= interactRadius && Input.GetMouseButtonDown(1))
        {
            if (canInteract)
            {
                InventoryManager.Instance?.AddItem(itemName); // 归一化在 InventoryManager 内完成
                DropItem();                                  
                canInteract = false;
                Invoke(nameof(ResetInteract), 0.5f); // 简单冷却
            }
        }
    }

    void DropItem()
    {
        if (dropPrefab == null) return;

        Vector2 dropPos = new Vector2(
            transform.position.x + Random.Range(-0.5f, 0.5f),
            transform.position.y + Random.Range(-0.5f, 0.5f)
        );

        GameObject drop = Instantiate(dropPrefab, dropPos, Quaternion.identity);
        Destroy(drop, 1.5f);
    }

    void ResetInteract()
    {
        canInteract = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}