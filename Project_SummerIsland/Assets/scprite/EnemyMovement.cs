using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    //????????????????????????????enemyManager??????????????????enemy???????????manger??
    public int health = 3;
    public float moveSpeed = 1f;
    public Collider2D moveArea;
    public EnemyManager.EnemyType type;
    public EnemyManager manager;
    public GameObject dropPrefab;

    private Vector2 targetPos;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        Bounds bounds = moveArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        targetPos = new Vector2(x, y);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SpawnDrop();
        ScoreManager.Instance?.AddScore(type.scoreValue);
        type.instances.Remove(gameObject); // ? [??]?????????
        Destroy(gameObject);
        manager.StartCoroutine(manager.RespawnEnemy(type)); // ? [??]???????
    }

    void SpawnDrop()
    {
        if (dropPrefab != null)
        {
            GameObject drop = Instantiate(dropPrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
            Destroy(drop, 2f);
            InventoryManager.Instance.AddItem(type.name); // ? [??]?????
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet")) // 
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}

