using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    // 玩家手持枪械的开火脚本
    public GameObject bulletPrefab;   // 子弹预制体
    public float fireRate = 0.2f;     // 连射间隔
    private float fireCooldown = 0f;  // 冷却计时

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // 1. 计算鼠标世界坐标
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 2. 计算枪朝向并旋转本物体
        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. 鼠标左键按下且冷却结束 -> 发射子弹
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;   // 子弹速度，可调

            fireCooldown = fireRate; // 重置冷却
        }
    }
}