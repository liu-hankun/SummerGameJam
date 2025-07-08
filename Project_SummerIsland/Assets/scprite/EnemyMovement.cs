using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyManager manager;
    public EnemyManager.EnemyType type;
    public float moveSpeed = 2f;
    public BoxCollider2D moveArea;

    Vector2 moveDir;
    float changeDirTimer;

    void Start()
    {
        ChooseNewDirection();
    }

    void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);

        // ???????? moveArea ?????
        Bounds bounds = moveArea.bounds;
        Vector3 pos = transform.position;
        if (pos.x < bounds.min.x || pos.x > bounds.max.x)
            moveDir.x = -moveDir.x;
        if (pos.y < bounds.min.y || pos.y > bounds.max.y)
            moveDir.y = -moveDir.y;

        changeDirTimer -= Time.deltaTime;
        if (changeDirTimer <= 0)
        {
            ChooseNewDirection();
        }
    }

    void ChooseNewDirection()
    {
        moveDir = Random.insideUnitCircle.normalized;
        changeDirTimer = Random.Range(1f, 3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ????
            type.instances.Remove(gameObject);
            Destroy(gameObject);

            // 3????
            manager.StartCoroutine(manager.RespawnEnemy(type));
        }
    }
}
