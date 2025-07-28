using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    //??????????????player???????????
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    private float fireCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // ? ????????
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // ? ?????????????
        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); // ?? ????????

        // ? ??????
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;

            fireCooldown = fireRate;
        }
    }
}
