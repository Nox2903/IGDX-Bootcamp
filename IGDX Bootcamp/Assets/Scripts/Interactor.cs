using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    private IInteractable interactableObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            interactableObject = interactable;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            if (interactableObject == interactable)
            {
                interactableObject = null;
            }
        }
    }
}
