using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    public Rigidbody2D rb; // 物理组件
    private Vector2 moveInput; // 输入方向
    public Animator animator; // 动画控制器

    // pick up

    private GameObject heldItem = null; // 当前持有的物品
    private GameObject nearbyFood = null; // 最近的食物
    public Transform headPoint;//举起食物的位置

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // 获取 Rigidbody2D
        animator = GetComponent<Animator>(); // 获取 Animator
    }

    void Update()
    {
        // 获取 WASD 输入
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // 设置移动动画（Run 或 Idle）
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("isRun", isMoving); // 根据移动状态切换 Run/Idle 动画

        // 鼠标左键攻击
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        // 移动角色
        rb.velocity = moveInput * moveSpeed;
    }

    //拿取食物
    void TryPickup()

    {
        if (heldItem == null && nearbyFood != null)
        {
            heldItem = nearbyFood;
            heldItem.transform.SetParent(headPoint); // 绑定到头部
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
            heldItem.GetComponent<Collider2D>().enabled = false;
            nearbyFood = null;
        }
        else if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem.GetComponent<Collider2D>().enabled = true;
            heldItem = null;
        }
    }

    // 检测进入食物范围
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food") && heldItem == null)
        {
            nearbyFood = other.gameObject;
        }
    }

    // 离开食物范围
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Food") && other.gameObject == nearbyFood)
        {
            nearbyFood = null;
        }
    }
}
