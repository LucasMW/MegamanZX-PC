using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex;
    public float moveSpeed;
    public float accelerationTimeAirborne;
    public float accelerationTimeGrounded;
    public float wallSlideSpeedMax;
    public float wallStickTime;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    Vector3 velocity;
    Controller2D controller;
    public Animator animator;
    private Collider2D playerCollider;
    private bool onGround;
    float jumpspeed;
    float gravity = -9.81f;
    float maxJumpVelocity;
    float minJumpVelocity;
    public float maxspeed;
    private GameObject ground;
    private bool FacingRight = true;
    float velocityXSmoothing;
    SpriteRenderer spriteRenderer;
    float timeToWallUnstick;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    bool noflip;
    public void Start()
    {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpspeed = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpspeed);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
     

    // Update is called once per frame
    
    void FixedUpdate()
    {

        wallSliding = false;
        CalculateVelocity();
        HandleWallSliding();
        

        
        animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        if (!noflip)
        {
            if (velocity.x > 0 && !FacingRight)
            {
                Flip();
            }
            else if (velocity.x < 0 && FacingRight)
            {
                Flip();
            }
        }

        if (animator.GetBool("NormAtk"))
        {
            noflip = true;
        }
        else if (!animator.GetBool("NormAtk"))
        {



            controller.Move(velocity * Time.deltaTime, directionalInput);
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
                animator.SetBool("On Ground", true);
                animator.SetBool("Wallslide", false);
                animator.ResetTrigger("Walljump");
                animator.SetBool("JumpAtk", false);
            }
            print("Move: " + velocity.x + " " + velocity.y);
        }

        if (!controller.collisions.below)
        {
            animator.SetBool("On Ground", false);
            onGround = false;
        }

    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }
    public void OnMeleeInputDown()
    {
        
        if (controller.collisions.below && Mathf.Abs(velocity.x) < 0.0001f)
        {
            animator.SetTrigger("Attack");
            animator.SetBool("NormAtk", true);
        }
        if (!controller.collisions.below)
        {
            animator.SetBool("JumpAtk", true);
        }

    }
    public void OnMeleeInputUp()
    {
        
        if (!controller.collisions.below)
        {
            animator.SetBool("JumpAtk", false);
        }
    }
    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            animator.SetBool("Wallslide", true);

            if (wallDirX == directionalInput.x)
            {

                animator.SetTrigger("Walljump");
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {

                animator.SetTrigger("Jump");
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
                print("JumpOff: " + velocity.x + " " + velocity.y);
            }
            else
            {

                animator.SetTrigger("Wallleap");
                animator.SetBool("Wallslide", false);
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        } else
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            animator.SetTrigger("Jump");
            animator.SetBool("On Ground", false);
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity && !wallSliding)
        {
            animator.SetTrigger("Jump");
            animator.SetBool("On Ground", false);
            velocity.y = minJumpVelocity;
        }
    }
    public void OnFallInput()
    {
        
        if (wallSliding)
        {
            velocity.y = -2 * wallSlideSpeedMax;
        }
        else
        {
            velocity.y += 2 * gravity * Time.deltaTime;
        }
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            animator.ResetTrigger("Jump");
            animator.SetBool("Wallslide", true);
            animator.SetBool("JumpAtk", false);
            if (velocity.y < -wallSlideSpeedMax)
            {
                
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    velocity.y = -2 * wallSlideSpeedMax;
                }
                else
                {
                    velocity.y = -wallSlideSpeedMax;
                }
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                animator.SetBool("Wallslide", false);
                timeToWallUnstick = wallStickTime;
            }

        }
        if(wallSliding == false)
        {
            animator.SetBool("Wallslide", false);
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }



    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;
        // Multiply the player's x local scale by -1.
        /*Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;*/
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    private void startNormAtk()
    {
        animator.SetBool("NormAtk", true);
    }
    private void endNormAtk()
    {
        animator.SetBool("NormAtk", false);
        animator.ResetTrigger("Attack");
        allowflip();
    }
    private void endwallslide()
    {
        animator.SetBool("Wallslide", false);
        allowflip();
    }
    private void allowflip()
    {
        noflip = false;
    }
    private void disallowflip()
    {
        noflip = true;
    }
}

