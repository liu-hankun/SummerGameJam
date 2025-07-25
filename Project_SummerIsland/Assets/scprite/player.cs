using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 moveInput;
    public Animator animator;

    private IKitchenTool nearbyTool = null;
    private IKitchenTool nearbyAssemblyTool = null;

    private GameObject heldItem = null;
    private GameObject nearbyFood = null;
    public Transform headPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool("isRun", isMoving);

        // E键：与厨房工具互动（如冰箱）
        if (Input.GetKeyDown(KeyCode.E) && nearbyTool != null && heldItem == null)
        {
            nearbyTool.Interact(this);
        }

        // Q键：组装台组装食材
        if (Input.GetKeyDown(KeyCode.Q) && nearbyAssemblyTool != null)
        {
            nearbyAssemblyTool.Interact(this);
        }

        // 鼠标左键：攻击动画
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        // 空格键：拾取或放下物品
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryPickup();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void TryPickup()
    {
        if (heldItem == null && nearbyFood != null)
        {
            heldItem = nearbyFood;
            heldItem.transform.SetParent(headPoint);
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

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }

    public void ReceiveItem(GameObject itemPrefab)
    {
        if (heldItem != null) return;

        GameObject item = Instantiate(itemPrefab, headPoint.position, Quaternion.identity);
        item.transform.SetParent(headPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.GetComponent<Collider2D>().enabled = false;

        heldItem = item;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food") && heldItem == null)
        {
            nearbyFood = other.gameObject;
        }

        if (other.CompareTag("KitchenObject"))
        {
            nearbyTool = other.GetComponent<IKitchenTool>();
        }

        if (other.CompareTag("AssemblyTable"))
        {
            nearbyAssemblyTool = other.GetComponent<IKitchenTool>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Food") && other.gameObject == nearbyFood)
        {
            nearbyFood = null;
        }

        if (other.CompareTag("KitchenObject"))
        {
            if (other.GetComponent<IKitchenTool>() == nearbyTool)
            {
                nearbyTool = null;
            }
        }

        if (other.CompareTag("AssemblyTable"))
        {
            if (other.GetComponent<IKitchenTool>() == nearbyAssemblyTool)
            {
                nearbyAssemblyTool = null;
            }
        }
    }
}