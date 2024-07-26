using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    public GameObject popUp;
    public GameObject PopUp => popUp;
    public GameObject popUp2;

    public float heldSpeed = 5f;
    public float heldSprintSpeed = 7f;
    public float heldJumpForce = 8f;
    public Transform holdPoint;
    public float throwForce = 10f;

    public GameObject heldItem;
    private Transform originalParent;
    public SpriteRenderer playerSpriteRenderer;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Interactor interactor;

    void Start()
    {
        originalParent = transform.parent;
        playerData = holdPoint.GetComponentInParent<PlayerData>();
        interactor = holdPoint.GetComponentInParent<Interactor>();
    }

    public void Interact()
    {
        if (heldItem == null)
        {
            PickupItem();
        }
    }

    public void UnInteract()
    {
        if (heldItem != null)
        {
            DropItem();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && heldItem != null)
        {
            ThrowItem();
        }
    }

    void PickupItem()
    {
        Debug.Log("Picking up item: " + gameObject.name);
        heldItem = gameObject;
        playerData.DisableJump();
        popUp2.SetActive(true);
        ChangePopUpXPosition(0.15f);
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.GetComponent<Collider>().enabled = false;
        heldItem.transform.position = holdPoint.position;
        heldItem.transform.parent = holdPoint;
        SetPlayerStats();
    }

    public void DropItem()
    {
        Debug.Log("Dropping item: " + heldItem.name);
        playerData.EnableJump();
        popUp2.SetActive(false);
        ChangePopUpXPosition(0.0f);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Collider>().enabled = true;
        heldItem.transform.parent = originalParent;
        heldItem = null;
        ResetPlayerStats();
    }

    public void ThrowItem()
    {
        Debug.Log("Throwing item: " + heldItem.name);
        Rigidbody itemRb = heldItem.GetComponent<Rigidbody>();
        itemRb.isKinematic = false;
        heldItem.GetComponent<Collider>().enabled = true;
        itemRb.transform.parent = originalParent;

        float throwDirection = playerSpriteRenderer.flipX ? -1 : 1;
        itemRb.AddForce(new Vector3((throwDirection * 0.5f), 1f, 0) * throwForce, ForceMode.VelocityChange);

        heldItem = null;
        playerData.EnableJump();
        interactor.isInteracting = false;
        popUp2.SetActive(false);
        ChangePopUpXPosition(0.0f);
        ResetPlayerStats();
    }

    void ChangePopUpXPosition(float newX)
    {
        Vector3 currentPosition = popUp.transform.localPosition;
        popUp.transform.localPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
    }

    public void SetPlayerStats()
    {
        if (playerData != null)
        {
            playerData.speed = heldSpeed;
            playerData.sprintSpeed = heldSprintSpeed;
            playerData.jumpForce = heldJumpForce;
        }
    }

    public void ResetPlayerStats()
    {
        if (playerData != null)
        {
            playerData.speed = playerData.resetSpeed;
            playerData.sprintSpeed = playerData.resetSprintSpeed;
            playerData.jumpForce = playerData.resetJumpForce;
        }
    }
}
