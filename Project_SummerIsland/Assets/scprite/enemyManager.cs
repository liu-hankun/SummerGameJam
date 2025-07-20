using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string name;
        public GameObject prefab;
        public float moveSpeed;
        public GameObject dropPrefab; // ? ??????????
        public List<GameObject> instances = new List<GameObject>();
    }

    public List<EnemyType> enemyTypes;
    public int maxTotalEnemies = 5;
    public BoxCollider2D spawnArea;

    void Start()
    {
        foreach (EnemyType type in enemyTypes)
        {
            SpawnEnemy(type); // ??????????
        }

        while (GetTotalEnemyCount() < maxTotalEnemies)
        {
            int randIndex = Random.Range(0, enemyTypes.Count);
            SpawnEnemy(enemyTypes[randIndex]);
        }
    }

    void SpawnEnemy(EnemyType type)
    {
        Vector2 spawnPos = GetRandomPositionInArea();
        GameObject enemy = Instantiate(type.prefab, spawnPos, Quaternion.identity);
        type.instances.Add(enemy);

        // ? ?? EnemyMovement ????
        EnemyMovement em = enemy.AddComponent<EnemyMovement>();
        em.manager = this;
        em.type = type;
        em.moveSpeed = type.moveSpeed;
        em.moveArea = spawnArea;
        em.dropPrefab = type.dropPrefab;
    }

    Vector2 GetRandomPositionInArea()
    {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

    int GetTotalEnemyCount()
    {
        int count = 0;
        foreach (EnemyType type in enemyTypes)
        {
            count += type.instances.Count;
        }
        return count;
    }

    public IEnumerator RespawnEnemy(EnemyType destroyedType)
    {
        yield return new WaitForSeconds(3f);

        if (destroyedType.instances.Count == 0)
        {
            SpawnEnemy(destroyedType);
        }
        else
        {
            EnemyType spawnType;
            do
            {
                int randIndex = Random.Range(0, enemyTypes.Count);
                spawnType = enemyTypes[randIndex];
            } while (spawnType == destroyedType && enemyTypes.Count > 1);

            SpawnEnemy(spawnType);
        }
    }
}

