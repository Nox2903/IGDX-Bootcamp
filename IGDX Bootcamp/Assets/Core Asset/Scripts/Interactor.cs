using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    GameObject PopUp { get; }
    void Interact();
    void UnInteract();
}

public class Interactor : MonoBehaviour
{
    public static Interactor instance;
    private IInteractable interactableObject;
    private Transform playerTransform;
    public Animator animator;
    public bool isInteracting = false;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerTransform = this.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null && animator.GetBool("IsInAir") == false)
        {
            if (!isInteracting)
            {
                Debug.Log("pressed interact button.");
                interactableObject.Interact();
                Debug.Log("1");
            }
            else
            {
                Debug.Log("pressed interact button.");
                interactableObject.UnInteract();
                Debug.Log("2");

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactableObject = interactable;
            interactableObject.PopUp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (interactableObject == interactable)
            {
                interactableObject.PopUp.SetActive(false);
                interactableObject = null;
            }
        }
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
}
