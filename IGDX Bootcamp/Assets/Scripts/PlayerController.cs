using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float resetSpeed;
    public float sprintSpeed;
    public float resetSprintSpeed;
    public float jumpForce;
    public float resetJumpForce;
    public float groundDist;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public GameObject lightPlayer;
    public Transform holdPoint;
    public Animator animator;

    public bool isGrounded;
    public PickableItem pickableItem;

    public float acceleration = 10f;
    public float deceleration = 10f;
    private float currentSpeed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        currentSpeed = 0f;
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
            if (hit.collider != null && Mathf.Abs(rb.velocity.y) <= 0.01f)
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
        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        float targetSpeed = (x != 0 || z != 0) ? (isSprinting ? sprintSpeed : speed) : 0f;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (targetSpeed > currentSpeed ? acceleration : deceleration) * Time.deltaTime);
        
        if (x != 0 || z != 0)
        {
            rb.velocity = new Vector3(moveDir.x * currentSpeed, rb.velocity.y, moveDir.z * currentSpeed);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsSprinting", isSprinting);

            if (x < 0)
            {
                sr.flipX = true;
                // Change lightPlayer's x position to negative
                Vector3 lightPlayerPos = lightPlayer.transform.localPosition;
                lightPlayerPos.x = Mathf.Abs(lightPlayerPos.x);
                lightPlayer.transform.localPosition = lightPlayerPos;

                Vector3 holdPointPos = holdPoint.transform.localPosition;
                holdPointPos.x = Mathf.Abs(holdPointPos.x);
                holdPoint.transform.localPosition = holdPointPos;
            }
            else if (x > 0)
            {
                sr.flipX = false;
                // Change lightPlayer's x position to positive
                Vector3 lightPlayerPos = lightPlayer.transform.localPosition;
                lightPlayerPos.x = Mathf.Abs(lightPlayerPos.x) * -1;
                lightPlayer.transform.localPosition = lightPlayerPos;

                Vector3 holdPointPos = holdPoint.transform.localPosition;
                holdPointPos.x = Mathf.Abs(holdPointPos.x) * -1;
                holdPoint.transform.localPosition = holdPointPos;
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
        else
        {
            isGrounded = true;
        }
    }
}
