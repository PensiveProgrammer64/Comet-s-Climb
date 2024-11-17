
using UnityEditor.Rendering;
using UnityEditor.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;
    [SerializeField] private float chargeRate = 0f;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private Transform[] groundCheckPoints;
    [SerializeField] private Transform[] leftWallCheckPoints;
    [SerializeField] private Transform[] rightWallCheckPoints;
    [SerializeField] private Transform[] ceilingCheckPoints;
    [SerializeField] private LayerMask detectionLayerMask;
   

    private bool isGrounded, isChargingJump, isJumping, isTouchingLeftWall, isTouchingRightWall, isTouchingCeiling;
    private float currentJumpForce = 0f;
    private Vector2 lastMovementDirection = Vector2.right;
    private float lastWalkingDirection = 5f;


    private enum WallSide { None, Left, Right }
    private WallSide lastWallSideTouched = WallSide.None;

    void Start() 
    { 
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
       
        HandleAnimation();
        float preCollisionXVelocity = rb.velocity.x;
        isGrounded = CheckMultipleSurfaces(groundCheckPoints, Vector2.down, out Vector2 groundNormal) && Mathf.Approximately(groundNormal.y, 1);
        isTouchingLeftWall = CheckMultipleSurfaces(leftWallCheckPoints, Vector2.left, out Vector2 leftWallNormal) && Mathf.Approximately(leftWallNormal.x, 1);
        isTouchingRightWall = CheckMultipleSurfaces(rightWallCheckPoints, Vector2.right, out Vector2 rightWallNormal) && Mathf.Approximately(rightWallNormal.x, -1);
        isTouchingCeiling = CheckMultipleSurfaces(ceilingCheckPoints, Vector2.up, out Vector2 ceilingNormal) && Mathf.Approximately(ceilingNormal.y, -1);

        if (isGrounded && !isJumping && !isChargingJump) movement.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        if ((movement.x > 0 && lastWalkingDirection < 0) || (movement.x < 0 && lastWalkingDirection > 0))
        {
            Flip();
        }
        if (movement.x != 0)
        {
            lastWalkingDirection = movement.x;
        }
        if (isTouchingLeftWall && lastWallSideTouched != WallSide.Left)
        {
            ReverseHorizontalVelocity(preCollisionXVelocity);
            lastWallSideTouched = WallSide.Left;
        }
        else if (isTouchingRightWall && lastWallSideTouched != WallSide.Right)
        {
            ReverseHorizontalVelocity(preCollisionXVelocity);
            lastWallSideTouched = WallSide.Right;
        }

        if (isTouchingCeiling) StopYMovement();

        if (isGrounded && isJumping) { isJumping = false; lastWallSideTouched = WallSide.None; }

        if (isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                isChargingJump = true;
                rb.velocity = new Vector2(0, rb.velocity.y);
                currentJumpForce += chargeRate * Time.deltaTime;
                currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxJumpForce);
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                if (horizontalInput != 0) lastMovementDirection = new Vector2(horizontalInput, 0).normalized;
            }
            else if (isChargingJump && Input.GetButtonUp("Jump")) { Jump(); isChargingJump = false; }
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded && !isJumping && !isChargingJump)
            rb.velocity = new Vector2(movement.x, rb.velocity.y);
        if(Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector2 (4, 82);
        }
    }

    private void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector2(lastMovementDirection.x * movementSpeed, currentJumpForce);
        currentJumpForce = 0;
    }

    private void ReverseHorizontalVelocity(float preCollisionXVelocity)
    {
        if (preCollisionXVelocity != 0) rb.velocity = new Vector2(-preCollisionXVelocity, rb.velocity.y);
    }

    private void StopYMovement() { if (rb.velocity.y > 0) rb.velocity = new Vector2(rb.velocity.x, 0); }

    private bool CheckMultipleSurfaces(Transform[] checkPoints, Vector2 direction, out Vector2 normal)
    {
        normal = Vector2.zero;
        foreach (var point in checkPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.position, direction, checkRadius, detectionLayerMask);
            if (hit) { normal = hit.normal; return true; }
        }
        return false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var point in groundCheckPoints) Gizmos.DrawWireSphere(point.position, checkRadius);
        foreach (var point in leftWallCheckPoints) Gizmos.DrawWireSphere(point.position, checkRadius);
        foreach (var point in rightWallCheckPoints) Gizmos.DrawWireSphere(point.position, checkRadius);
        foreach (var point in ceilingCheckPoints) Gizmos.DrawWireSphere(point.position, checkRadius);
    }
    void HandleAnimation ()
    {
        animator.SetBool("isWalking", movement.x !=0f);
        animator.SetBool("isJumping", !isGrounded);
    }
    void Flip ()
    {
        Debug.Log("Player Sprite Begin Flip Method");
        spriteRenderer.flipX = !spriteRenderer.flipX;
        Debug.Log("Player Sprite End Flip Method");
    }
}