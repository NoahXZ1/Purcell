using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;
    public float acceleration = 60f;
    public float deceleration = 70f;
    public float airControlMultiplier = 0.6f;

    [Header("Jump (fixed height)")]
    public float jumpVelocity = 12f;   //the speed of jumping

    [Header("Ground Check")]
    public Transform groundCheck;      //The sub-object GroundCheck
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;      

    Rigidbody2D rb;
    bool isGrounded;
    bool canJump;  //Keep the player can only jump when touch the ground.                     
    float inputX;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        //The input in X-axis
        inputX = Input.GetAxisRaw("Horizontal");

        //To test whether its on the ground or not. 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded) canJump = true;

        //Start jump. 
        if (Input.GetButtonDown("Jump") && canJump && isGrounded)
            Jump();
    }

    void FixedUpdate()
    {
        float target = inputX * moveSpeed;
        float speedDiff = target - rb.velocity.x;
        float accel = (Mathf.Abs(target) > 0.01f) ? acceleration : deceleration;
        if (!isGrounded) accel *= airControlMultiplier;

        rb.AddForce(new Vector2(accel * speedDiff, 0f) * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    void Jump()
    {
        canJump = false;
        //Fixed jumping start speed. 
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);  
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
