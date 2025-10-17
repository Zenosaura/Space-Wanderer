using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    //for animation
    public Animator animator;
    private float previousX;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        previousX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {   

        float currentX = transform.position.x;
        
        //grounded?
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        
        // get player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // is player above enemy?
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 7f, 1 << player.gameObject.layer);

        if(isGrounded){
            //chase player
            rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

            

            //If ground
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            
            //If gap
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
           
            //If platform above
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 7f, groundLayer);
        
            
            // Jump if there's a gap head && no ground infront
            //else if there's player above and platform above
            
            if (!groundInFront.collider && !gapAhead.collider){
                shouldJump = true;
            }
            else if(isPlayerAbove && platformAbove.collider){
                shouldJump = true;
            }
        }
        // flip
        if(currentX > previousX){
            spriteRenderer.flipX = true;
        }
        else if (currentX < previousX){
            spriteRenderer.flipX = false;
        }
        previousX = currentX;



        //animation
        animator.SetFloat("magnitude", rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if(isGrounded && shouldJump){
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;

            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }





}
