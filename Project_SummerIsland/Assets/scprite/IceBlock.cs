using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    //??????????????????????
    public string itemName = "Ice";
    public float interactRadius = 2f;
    public GameObject dropPrefab;  

    private Transform player;
    private bool canInteract = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactRadius && Input.GetMouseButtonDown(1))
        {
            if (canInteract)
            {
                InventoryManager.Instance.AddItem(itemName); 
                DropItem();                                  
                canInteract = false;
                Invoke(nameof(ResetInteract), 0.5f);
            }
        }
    }

    void DropItem()
    {
        if (dropPrefab == null) return;

        
        Vector2 dropPos = new Vector2(
            transform.position.x + Random.Range(-3f, 3f),
            transform.position.y - Random.Range(5f, 6f)
        );

        GameObject drop = Instantiate(dropPrefab, dropPos, Quaternion.identity);

        
        Destroy(drop, 2f);
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


