using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private float currentSpeed;
    
    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayerData.instance.rb = gameObject.GetComponent<Rigidbody>();
        currentSpeed = 0f;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdatePositionOnTerrain();
    }

    void UpdatePositionOnTerrain()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, PlayerData.instance.terrainLayer))
        {
            if (hit.collider != null && Mathf.Abs(PlayerData.instance.rb.velocity.y) <= 0.01f)
            {
                PlayerData.instance.isGrounded = true;
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + PlayerData.instance.groundDist;
                PlayerData.instance.animator.SetBool("IsInAir", false);
                PlayerData.instance.currentState = PlayerData.PlayerState.Idle;
            }
            else
            {
                PlayerData.instance.isGrounded = false;
                PlayerData.instance.animator.SetBool("IsInAir", true);
                PlayerData.instance.currentState = PlayerData.PlayerState.InAir;
            }
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool isSprinting = PlayerData.instance.IsSprintEnabled() && Input.GetKey(KeyCode.LeftShift);
        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        float targetSpeed = (x != 0 || z != 0) ? (isSprinting ? PlayerData.instance.sprintSpeed : PlayerData.instance.speed) : 0f;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (targetSpeed > currentSpeed ? PlayerData.instance.acceleration : PlayerData.instance.deceleration) * Time.deltaTime);

        if (x != 0 || z != 0)
        {
            PlayerData.instance.rb.velocity = new Vector3(moveDir.x * currentSpeed, PlayerData.instance.rb.velocity.y, moveDir.z * currentSpeed);
            PlayerData.instance.animator.SetBool("IsWalking", true);
            PlayerData.instance.animator.SetBool("IsSprinting", isSprinting);
            PlayerData.instance.currentState = isSprinting ? PlayerData.PlayerState.Sprinting : PlayerData.PlayerState.Walking;

            if (x < 0)
            {
                if (!PlayerData.instance.IsFlipXFrozen())
                {
                    PlayerData.instance.sr.flipX = true;
                    Vector3 lightPlayerPos = PlayerData.instance.lightPlayer.transform.localPosition;
                    lightPlayerPos.x = Mathf.Abs(lightPlayerPos.x);
                    PlayerData.instance.lightPlayer.transform.localPosition = lightPlayerPos;

                    Vector3 holdPointPos = PlayerData.instance.holdPoint.transform.localPosition;
                    holdPointPos.x = Mathf.Abs(holdPointPos.x);
                    PlayerData.instance.holdPoint.transform.localPosition = holdPointPos;
                }
            }
            else if (x > 0)
            {
                if (!PlayerData.instance.IsFlipXFrozen())
                {
                    PlayerData.instance.sr.flipX = false;
                    Vector3 lightPlayerPos = PlayerData.instance.lightPlayer.transform.localPosition;
                    lightPlayerPos.x = Mathf.Abs(lightPlayerPos.x) * -1;
                    PlayerData.instance.lightPlayer.transform.localPosition = lightPlayerPos;

                    Vector3 holdPointPos = PlayerData.instance.holdPoint.transform.localPosition;
                    holdPointPos.x = Mathf.Abs(holdPointPos.x) * -1;
                    PlayerData.instance.holdPoint.transform.localPosition = holdPointPos;
                }
            }
        }
        else
        {
            PlayerData.instance.rb.velocity = new Vector3(0, PlayerData.instance.rb.velocity.y, 0);
            PlayerData.instance.animator.SetBool("IsWalking", false);
            PlayerData.instance.animator.SetBool("IsSprinting", false);
            PlayerData.instance.currentState = PlayerData.PlayerState.Idle;
        }
    }

    void HandleJump()
    {
        if (PlayerData.instance.IsJumpEnabled() && Input.GetButtonDown("Jump") && PlayerData.instance.isGrounded)
        {
            Debug.Log("pressed space");
            PlayerData.instance.rb.AddForce(Vector3.up * PlayerData.instance.jumpForce, ForceMode.Impulse);
            PlayerData.instance.isGrounded = false;
            PlayerData.instance.animator.SetBool("IsInAir", true);
            PlayerData.instance.currentState = PlayerData.PlayerState.Jumping;
        }
    }
}
