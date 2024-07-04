using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public float heldSpeed = 5f;
    public float heldSprintSpeed = 5f;
    public float heldJumpForce = 8f;
    public Transform holdPoint;  // The point where the item will be held
    public float throwForce = 10f;  // The force applied when throwing the item

    public GameObject heldItem;
    private GameObject pickableItem;
    public SpriteRenderer playerSpriteRenderer;  // Reference to the player's SpriteRenderer component

    void Start()
    {
        // playerSpriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component from the player
    }

    void Update()
    {
        // Check for pickup or drop/throw input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null && pickableItem != null)
            {
                PickupItem(pickableItem);
            }
            else if (heldItem != null)
            {
                DropItem();
            }
        }

        // Check for throw input
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            ThrowItem();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            pickableItem = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            pickableItem = null;
        }
    }

    void PickupItem(GameObject item)
    {
        Debug.Log("Picking up item: " + item.name);
        heldItem = item;
        heldItem.GetComponent<Rigidbody>().isKinematic = true;  // Disable physics while holding
        heldItem.GetComponent<Collider>().enabled = false;  // Disable the collider
        heldItem.transform.position = holdPoint.position;
        heldItem.transform.parent = holdPoint;  // Parent the item to the hold point
    }

    void DropItem()
    {
        Debug.Log("Dropping item: " + heldItem.name);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;  // Re-enable physics
        heldItem.GetComponent<Collider>().enabled = true;  // Re-enable the collider
        heldItem.transform.parent = null;  // Unparent the item
        heldItem = null;
    }

    void ThrowItem()
    {
        Debug.Log("Throwing item: " + heldItem.name);
        Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
        itemRb.isKinematic = false;  // Re-enable physics
        heldItem.GetComponent<Collider>().enabled = true;  // Re-enable the collider
        itemRb.transform.parent = null;  // Unparent the item

        // Determine throw direction based on player's facing direction
        float throwDirection = playerSpriteRenderer.flipX ? -1 : 1;  // Check if player is flipped

        // Apply force in the x-direction
        itemRb.AddForce(new Vector3(throwDirection, 0, 0) * throwForce, ForceMode.VelocityChange);

        heldItem = null;
    }
}
