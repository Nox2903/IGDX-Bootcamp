using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public static PlayerData instance;
    public enum PlayerState
    {
        Idle,
        Walking,
        Sprinting,
        Jumping,
        InAir
    }

    [Header("Speed Data")]
    public float speed;
    public float resetSpeed;
    public float sprintSpeed;
    public float resetSprintSpeed;

    [Header("Jump Data")]
    public float jumpForce;
    public float resetJumpForce;
    public float groundDist;
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public GameObject lightPlayer;
    public Transform holdPoint;
    public Animator animator;

    public PlayerState currentState;
    public PickableItem pickableItem;

    public float acceleration = 10f;
    public float deceleration = 10f;
    public bool isGrounded;

    [SerializeField] private bool jumpEnabled = true;
    private bool sprintEnabled = true;
    private bool flipXFrozen = false;

    public void DisableJump() => jumpEnabled = false;
    public void EnableJump() => jumpEnabled = true;
    public void DisableSprinting() => sprintEnabled = false;
    public void EnableSprinting() => sprintEnabled = true;
    public void FreezeFlipX(bool freeze) => flipXFrozen = freeze;

    public bool IsJumpEnabled() => jumpEnabled;
    public bool IsSprintEnabled() => sprintEnabled;
    public bool IsFlipXFrozen() => flipXFrozen;


    public void Awake()
    {
        instance = this;
    }
}
