using System;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool doubleJump;
    bool facingRight = true;
    public Animator animator;
    
    public ParticleSystem smokeFX;

    
    [Header("Movement")]
    public float moveSpeed = 5f ;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;

    [Header("Gravity")]
	public float baseGravity = 2f;
	public float maxFallSpeed =18f;
	public float fallSpeedMultiplier = 2f;
    

    [Header("GroundCheck")]
    public Transform groundcheckPos;
    public Vector2 groundcheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool onGround;

    [Header("WallCheck")]
    public Transform wallcheckPos;
    public Vector2 wallcheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;


    [Header("WallMovement")]
    public float wallSlideSpeed =2;
    public bool IsWallSliding;
    //wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    
    
    

    // Update is called once per frame
    void Update()
    {
        
        
             
        IsGrounded();
        // Gravity();  
        
        JumpMechanic();
        WallSlide();
        DoWallJump();
        
        
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }
        
        animator.SetFloat("yVel", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("WallSliding", IsWallSliding);
        
    }

    private void JumpMechanic()
    {
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower / 2 );

                doubleJump = !doubleJump;

                
            }
            JumpFX();
        } 

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f );
            JumpFX();
        }

        // wall jumping
        if (Input.GetButtonUp("Jump") && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x , wallJumpPower.y); // jump away from wall
            wallJumpTimer = 0;
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); // wall jump = 0.5f -- jump again = 0.6f;
            JumpFX();
            
            // make it flip when wall jump
            if (transform.localScale.x != wallJumpDirection)
            {
                facingRight = !facingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            }
        }


    }

    private void JumpFX(){
            animator.SetTrigger("Jump");
            smokeFX.Play();
    }



    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x ;
    }


private bool IsGrounded()
{ 
    if(Physics2D.OverlapBox(groundcheckPos.position, groundcheckSize, 0, groundLayer))
    {
    return true;
    }
    return false;
    
}

private bool WallCheck()
{
  return Physics2D.OverlapBox(wallcheckPos.position, wallcheckSize, 0, wallLayer);
}
private void Flip()
{
   if (facingRight && horizontalMovement < 0 || !facingRight && horizontalMovement > 0)
   {
        facingRight = !facingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f;
        transform.localScale = ls;

        
   } 
}
   
private void Gravity()
	{
		if (rb.velocity.y < 0)
		{
			rb.gravityScale = baseGravity * fallSpeedMultiplier; // fall faster
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y - maxFallSpeed));
		}
		else
		{
			rb.gravityScale = baseGravity;
		}
	}

private void WallSlide()
{
    // not grounded & on a wall & no movement
    if (!IsGrounded() & WallCheck() & horizontalMovement !=0)
    {
        IsWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
    }
    else
    {
        IsWallSliding = false;
    }
}

private void DoWallJump()
{
    if (IsWallSliding)
    {
        isWallJumping = false;
        wallJumpDirection = -transform.localScale.x;
        wallJumpTimer = wallJumpTime;

        CancelInvoke(nameof(CancelWallJump));
    }
    else if (wallJumpTimer> 0f)
    {
        wallJumpTimer -= Time.deltaTime;
    }
}

private void CancelWallJump()
{
    isWallJumping = false;
}


private void OnDrawGizmosSelected() 
{
    Gizmos.color = Color.white;
    Gizmos.DrawWireCube(groundcheckPos.position, groundcheckSize);

    Gizmos.color = Color.blue;
    Gizmos.DrawWireCube(wallcheckPos.position, wallcheckSize);
}

    
}
    	

    













