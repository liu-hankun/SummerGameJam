using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int health = 3;
    public float moveSpeed = 1f;
    public Collider2D moveArea;
    public EnemyManager.EnemyType type;  // ✅ 完整类型名
    public EnemyManager manager;
    public GameObject dropPrefab;


    private Vector2 targetPos;

    void Start()
    {
        PickRandomTarget();
    }

    void Update()
    {
        MoveToTarget();
    }

    void PickRandomTarget()
    {
        if (moveArea != null)
        {
            Bounds b = moveArea.bounds;
            targetPos = new Vector2(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y));
        }
        else
        {
            targetPos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            PickRandomTarget();
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 掉落视觉
        if (dropPrefab != null)
        {
           GameObject drop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
           Destroy(drop, 2f);
        }

        // ✅ 增加库存
        string itemName = (type != null && !string.IsNullOrEmpty(type.name)) ? type.name : "mango";
        int newCount = InventoryManager.Instance != null ? InventoryManager.Instance.AddItem(itemName) : -1;
        if (newCount >= 0)
            Debug.Log($"[Enemy] Killed '{itemName}', inventory now {newCount}.");
        else
            Debug.LogWarning("[Enemy] InventoryManager.Instance is null, cannot add item.");

        // 通知管理器重生
        if (manager != null)
    {
        manager.StartCoroutine(manager.RespawnEnemy(type));
    }
    Destroy(gameObject);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
  
}
