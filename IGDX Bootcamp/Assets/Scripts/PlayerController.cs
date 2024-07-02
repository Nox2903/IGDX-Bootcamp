using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sprintSpeed;
    public float jumpForce;
    public float groundDist;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator animator;

    public bool isGrounded;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdatePositionOnTerrain();
        HandleMovement();
        HandleJump();
    }

    void UpdatePositionOnTerrain()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null && rb.velocity.y <= 0 && rb.velocity.y >= 0)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                isGrounded = true;
                animator.SetBool("IsInAir", false);
            }
            else
            {
                isGrounded = false;
                animator.SetBool("IsInAir", true);
            }
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        Vector3 moveDir = new Vector3(x, 0, z);

        if (x != 0 || z != 0)
        {
            float currentSpeed = isSprinting ? sprintSpeed : speed;
            rb.velocity = new Vector3(moveDir.x * currentSpeed, rb.velocity.y, moveDir.z * currentSpeed);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsSprinting", isSprinting);

            if (x < 0)
            {
                sr.flipX = true;
            }
            else if (x > 0)
            {
                sr.flipX = false;
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsSprinting", false);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("IsInAir", true);
        }
    }
}
