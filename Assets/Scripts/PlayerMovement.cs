using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    //---------- Runtime params ----------
    [Header("Move (runtime)")]
    public float moveSpeed = 6f;
    public float acceleration = 60f;
    public float deceleration = 70f;
    public float airControlMultiplier = 0.6f;

    [Header("Jump (fixed height, runtime)")]
    public float jumpVelocity = 12f; //setting the initial veclority in Y-aixs directly. 

    [Header("Ground Check (runtime)")]
    public Transform groundCheck;          //swutch to the corrsponding groundcheck. 
    public float groundCheckRadius = 0.18f;
    public LayerMask groundLayer;          

    //---------- Forms ----------
    public enum PlayerForm { Human, Cat }

    [Header("Form State")]
    public PlayerForm currentForm = PlayerForm.Human;

    [Header("Human Form Settings")]
    public float humanMoveSpeed = 6f;
    public float humanAcceleration = 60f;
    public float humanDeceleration = 70f;
    public float humanAirControlMultiplier = 0.6f;
    public float humanJumpVelocity = 12f;
    public GameObject humanVisual;         //Player/HumanVisual
    public Collider2D humanCollider;       //Player/HumanCollider's Collider2D
    public Transform humanGroundCheck;     //Player/GroundCheck_Human
    public LayerMask humanVisionMask;      //This is for the asset of human when finished. 

    [Header("Cat Form Settings")]
    public float catMoveSpeed = 6.5f;
    public float catAcceleration = 60f;
    public float catDeceleration = 70f;
    public float catAirControlMultiplier = 0.65f;
    public float catJumpVelocity = 14f;    //Cat will have higher ability in jumping
    public GameObject catVisual;           //CatVisual
    public Collider2D catCollider;         //CatCollider's Collider2D(disabled initially)
    public Transform catGroundCheck;       //GroundCheck_Cat
    public LayerMask catVisionMask;        //This is for the asset of cat when finished. 

    // ---------- Internals ----------
    Rigidbody2D rb;
    bool isGrounded;
    bool canJump;
    float inputX;

    //External hooks: can be subscribed to in the future when doing UI/VFX/sound effects/view switching
    public event Action<PlayerForm> OnFormChanged;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //Initialize all visual/collision/parameters with currentForm
        ApplyForm(currentForm);
    }

    void Update()
    {
        //Switch between Cat and Human(press F)
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleForm();
        }

        //horizontal input
        inputX = Input.GetAxisRaw("Horizontal");

        //ground checking
        isGrounded = (groundCheck != null) &&
                     Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded) canJump = true;

        //Fixed height jump: Press the button immediately and must be on the ground
        if (Input.GetButtonDown("Jump") && canJump && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        float target = inputX * moveSpeed;
        float speedDiff = target - rb.velocity.x;
        float accel = (Mathf.Abs(target) > 0.01f) ? acceleration : deceleration;
        if (!isGrounded) accel *= airControlMultiplier;

        // Clamp so the impulse never overshoots the target velocity (prevents numerical instability
        // when accel * fixedDeltaTime > 2, which causes velocity to oscillate and diverge).
        float velocityChange = Mathf.Clamp(
            accel * speedDiff * Time.fixedDeltaTime,
            -Mathf.Abs(speedDiff),
            Mathf.Abs(speedDiff)
        );
        rb.AddForce(new Vector2(velocityChange, 0f), ForceMode2D.Impulse);
    }

    void Jump()
    {
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    // ---------- Form logic ----------
    void ToggleForm()
    {
        currentForm = (currentForm == PlayerForm.Human) ? PlayerForm.Cat : PlayerForm.Human;
        ApplyForm(currentForm);
        OnFormChanged?.Invoke(currentForm);
    }

    void ApplyForm(PlayerForm form)
    {
        //override the parameters
        if (form == PlayerForm.Human)
        {
            moveSpeed = humanMoveSpeed;
            acceleration = humanAcceleration;
            deceleration = humanDeceleration;
            airControlMultiplier = humanAirControlMultiplier;
            jumpVelocity = humanJumpVelocity;
            groundCheck = humanGroundCheck;
        }
        else
        {
            moveSpeed = catMoveSpeed;
            acceleration = catAcceleration;
            deceleration = catDeceleration;
            airControlMultiplier = catAirControlMultiplier;
            jumpVelocity = catJumpVelocity;
            groundCheck = catGroundCheck;
        }

        //Able/disable the human/cat body.
        if (humanVisual) humanVisual.SetActive(form == PlayerForm.Human);
        if (catVisual) catVisual.SetActive(form == PlayerForm.Cat);

        //Enable the corresponding Collider (both are attached to the Player or its children and share the same Rigidbody2D)
        if (humanCollider) humanCollider.enabled = (form == PlayerForm.Human);
        if (catCollider) catCollider.enabled = (form == PlayerForm.Cat);

        //Ensure the switch activated without delay. 
        Physics2D.SyncTransforms();
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
