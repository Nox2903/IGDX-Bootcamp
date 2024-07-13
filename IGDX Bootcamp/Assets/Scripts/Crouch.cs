using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ... rest of your code ...

    public float crouchHeight = 0.5f; // Adjust this value as needed
    public float crouchSpeedModifier = 0.5f; // Optional: Movement speed reduction while crouching
    public bool isCrouching;

    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        // ... rest of your Start code ...
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // ... rest of your Update code ...
        HandleCrouch();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Replace with your desired crouch input
        {
            isCrouching = !isCrouching;
        }

        float targetHeight = isCrouching ? crouchHeight : characterController.height;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * 10f); // Adjust lerp speed

        // Adjust capsule collider height and center (optional)
        capsuleCollider.height = characterController.height;
        capsuleCollider.center = new Vector3(capsuleCollider.center.x, characterController.height * 0.5f, capsuleCollider.center.z);

        // Adjust movement speed (optional)
        speed = isCrouching ? speed * crouchSpeedModifier : resetSpeed;
        sprintSpeed = isCrouching ? sprintSpeed * crouchSpeedModifier : resetSprintSpeed;

        // Trigger crouch animation (optional)
        animator.SetBool("IsCrouching", isCrouching);
    }
}

