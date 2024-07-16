using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullObject : MonoBehaviour, IInteractable
{
    public GameObject popUp;
    public BoxCollider boxCol;
    [SerializeField] private bool isInteracting = false;
    private Transform playerTransform;
    private Rigidbody rb;
    private FixedJoint fixedJoint;
    [SerializeField] private PlayerController playerController; // Reference to PlayerController script

    public GameObject PopUp
    {
        get { return popUp; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from this game object.");
        }

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController script not found on player object.");
        }
    }
    public void Interact()
    {
        rb.isKinematic = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController.DisableJump(); // Call method to disable jumping
        playerController.FreezeFlipX(true); // Freeze flipX value
        playerController.DisableSprinting();
        StartCoroutine(CreateFixedJointDelayed());
    }

    public void UnInteract()
    {
        DestroyFixedJoint();
        rb.isKinematic = true;
        playerController.EnableJump(); // Call method to enable jumping
        playerController.FreezeFlipX(false); // Unfreeze flipX value
        playerController.EnableSprinting();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyFixedJoint();
        }
    }

    private IEnumerator CreateFixedJointDelayed()
    {
        yield return new WaitForSeconds(0.1f); // Wait for the next fixed update frame
        CreateFixedJoint();
    }

    private void CreateFixedJoint()
    {
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = playerTransform.GetComponent<Rigidbody>();
    }

    private void DestroyFixedJoint()
    {
        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
            fixedJoint = null;
        }
    }
}
