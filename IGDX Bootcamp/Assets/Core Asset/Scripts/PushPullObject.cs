using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPullObject : MonoBehaviour, IInteractable
{
    public GameObject popUp;
    public BoxCollider boxCol;
    private Transform playerTransform;
    private Rigidbody rb;
    private FixedJoint fixedJoint;
    [SerializeField] private PlayerData playerData;

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

        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (playerData == null)
        {
            Debug.LogError("PlayerData script not found on player object.");
        }
    }
    
    public void Interact()
    {
        rb.isKinematic = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerData.DisableJump();
        playerData.FreezeFlipX(true);
        playerData.DisableSprinting();
        StartCoroutine(CreateFixedJointDelayed());
    }

    public void UnInteract()
    {
        DestroyFixedJoint();
        rb.isKinematic = true;
        playerData.EnableJump();
        playerData.FreezeFlipX(false);
        playerData.EnableSprinting();
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
        yield return new WaitForSeconds(0.1f);
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
