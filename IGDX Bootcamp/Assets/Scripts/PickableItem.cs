using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    public GameObject popUp;
    public GameObject PopUp => popUp; // Implementing the PopUp property
    public GameObject popUp2;
    public float heldSpeed = 5f;
    public float heldSprintSpeed = 7f;
    public float heldJumpForce = 8f;
    public Transform holdPoint;  // The point where the item will be held
    public float throwForce = 10f;  // The force applied when throwing the item

    public GameObject heldItem;
    private bool isPickedUp = false;  // Track if the item is currently picked up
    private Transform originalParent;
    public SpriteRenderer playerSpriteRenderer;  // Reference to the player's SpriteRenderer component

    void Start()
    {
        originalParent = transform.parent;
    }

    public void Interact()
    {
        if (heldItem == null)
        {
            PickupItem();
        }
        else
        {
            DropItem();
        }
    }

    void Update()
    {
        // Check for throw input
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            ThrowItem();
        }
    }

    void PickupItem()
    {
        //Debug.Log("Picking up item: " + gameObject.name);
        heldItem = gameObject;
        isPickedUp = true;
        popUp2.SetActive(true);
        ChangePopUpXPosition(0.15f); // Change the x coordinate to 0.15
        heldItem.GetComponent<Rigidbody>().isKinematic = true;  // Disable physics while holding
        heldItem.GetComponent<Collider>().enabled = false;  // Disable the collider
        heldItem.transform.position = holdPoint.position;
        heldItem.transform.parent = holdPoint;  // Parent the item to the hold point
        SetPlayerStats();
    }

    void DropItem()
    {
        Debug.Log("Dropping item: " + heldItem.name);
        isPickedUp = false;
        popUp2.SetActive(false);
        ChangePopUpXPosition(0.0f); // Change the x coordinate back to 0
        heldItem.GetComponent<Rigidbody>().isKinematic = false;  // Re-enable physics
        heldItem.GetComponent<Collider>().enabled = true;  // Re-enable the collider
        heldItem.transform.parent = originalParent;  // Reset the parent
        heldItem = null;
        ResetPlayerStats();
    }

    void ThrowItem()
    {
        Debug.Log("Throwing item: " + heldItem.name);
        Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
        itemRb.isKinematic = false;  // Re-enable physics
        heldItem.GetComponent<Collider>().enabled = true;  // Re-enable the collider
        itemRb.transform.parent = originalParent;  // Reset the parent

        // Determine throw direction based on player's facing direction
        float throwDirection = playerSpriteRenderer.flipX ? -1 : 1;  // Check if player is flipped

        // Apply force in the x-direction
        itemRb.AddForce(new Vector3((throwDirection * 0.5f), 1f, 0) * throwForce, ForceMode.VelocityChange);

        heldItem = null;
        isPickedUp = false;
        popUp2.SetActive(false);
        ChangePopUpXPosition(0.0f); // Change the x coordinate back to 0
        ResetPlayerStats();
    }

    void ChangePopUpXPosition(float newX)
    {
        Vector3 currentPosition = popUp.transform.localPosition;
        popUp.transform.localPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
    }
    
    public void SetPlayerStats()
    {
        var playerController = holdPoint.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = heldSpeed;
            playerController.sprintSpeed = heldSprintSpeed;
            playerController.jumpForce = heldJumpForce;
        }
    }

    public void ResetPlayerStats()
    {
        var playerController = holdPoint.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            playerController.speed = playerController.resetSpeed;
            playerController.sprintSpeed = playerController.resetSprintSpeed;
            playerController.jumpForce = playerController.resetJumpForce;
        }
    }
}
