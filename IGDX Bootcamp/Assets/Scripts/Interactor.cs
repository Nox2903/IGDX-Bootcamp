using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    GameObject PopUp { get; }
    void Interact();
}

public class Interactor : MonoBehaviour
{
    private IInteractable interactableObject;
    private Transform playerTransform;
    public bool isInteracting = false;

    void Start()
    {
        playerTransform = this.transform;
    }

    void Update()
    {
        if (!isInteracting && Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactableObject = interactable;
            interactableObject.PopUp.SetActive(true); // Activate the PopUp GameObject
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (interactableObject == interactable)
            {
                interactableObject.PopUp.SetActive(false); // Deactivate the PopUp GameObject
                interactableObject = null;
            }
        }
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }
}
