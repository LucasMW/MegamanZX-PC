using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
using DG.Tweening;
public class PlayerControl : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    private float baseGravity;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    public float dashSpeed;
    public float dashDistance;
    public Vector2 wallJump;
    public Vector2 wallLeap;
    public float totalDash;
    public float wallSlideSpeed;
    public float wallSlideSpeedSmoothingTime;
    private float wallSlideSpeedSmoothing;
    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private bool wallsliding;
    private float wallDirX;
    float Delayanimation = 1f;
    private bool walljumping = false;
    public bool wallgrab = false;
    public bool runatk;
    public float currentDash;
    public bool r_DashCDRStarted = false;
    public bool r_WallGrabbingStarted = false;
    private float dashTime;
    private GameObject currentDashBoost;
    public GameObject DashBoost;
    public GameObject DashDust;
    private GameObject currentDashDust;
    private bool DashBoostcreated;
    float currentDashCooldownTimer;
    float dashCooldown;
    private SoundManager soundManager;
    bool jumpatk;
    bool dashing;
    [HideInInspector]
    public bool canCDdash;
    public float DashCooldownTimer = 5;
    public GhostEffect ghost;
    bool jumpdashing;
    public float soundTimerset;
    private float soundTimer;
    void Awake()
    {
        
        currentDashCooldownTimer = DashCooldownTimer;
        baseGravity = gravity;
        dashTime = dashDistance / dashSpeed;
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        currentDashCooldownTimer = 0;
        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        currentDash = totalDash;
        r_DashCDRStarted = false;
        r_WallGrabbingStarted = false;
        soundTimer = soundTimerset;
    }


    #region Event Listeners
    private bool onewayplat;
    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.collider.tag == "One Way Plat") {
            onewayplat = true;
        }
        if (hit.collider.tag == "Ground")
        {
            onewayplat = false;
        }
        return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
        Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
    }


    void onTriggerExitEvent(Collider2D col)
    {
        Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion


    void Start()
    {
        soundManager = SoundManager._instance;
        if (soundManager == null)
        {
            Debug.LogError("No SoundManager found in Scene!!!!!");
        }
    }
    void MoveCalculation()
    {
        wallsliding = false;

        if (_controller.isGrounded)
        {
            jumpatk = false;
            gravity = baseGravity;
            walljumping = false;
            wallsliding = false;
            wallgrab = false;
            _velocity.y = 0;
            _animator.SetBool("On Ground", true);
            _animator.ResetTrigger("Walljump");
            _animator.SetBool("Wallslide", false);
            _animator.SetBool("JumpAtk", false);
            _animator.ResetTrigger("Jump");
            if (jumpdashing == true)
            {
                ghost.makeAfterimage = false;
                jumpdashing = false;
                dashing = false;
            }
        }
        if (!_controller.isGrounded)
        {
            _animator.SetBool("On Ground", false);
        }
        if (!wallgrab && !_animator.GetBool("NormAtk") && !_animator.GetBool("Dash") && !walljumping)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f && _velocity.x > 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                if (_controller.isGrounded)
                {

                    _animator.SetFloat("Speed", 1);
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f && _velocity.x < 0)
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                if (_controller.isGrounded)
                {

                    _animator.SetFloat("Speed", 1);
                }
            }
            else
            {
                normalizedHorizontalSpeed = 0;
                wallsliding = false;
                _animator.SetBool("Wallslide", false);
                if (_controller.isGrounded && !_animator.GetBool("DashAtk") )
                    _animator.SetFloat("Speed", 0);
            }
        }
        // we can only jump whilst grounded
        if (_controller.isGrounded && Input.GetKeyDown(KeyCode.UpArrow) && !_animator.GetBool("NormAtk") && !_animator.GetBool("RunAtk"))
        {
            wallsliding = false;
            _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            _animator.SetBool("On Ground", false);
            _animator.SetTrigger("Jump");
            soundManager.PlaySound("Jump");
            _animator.SetBool("DashAtk", false);
            if (dashing)
            {
                jumpdashing = true;
            }
            /*if (currentDashBoost != null)
            {
                currentDashBoost.SetActive(false);
                Destroy(currentDashBoost);
                Destroy(currentDashDust, 1f);
            }*/
        }

        //Wall sliding
        if ((_controller.collisionState.left || _controller.collisionState.right)
            && !_controller.isGrounded && _velocity.y <= 0.01)

        {
            _animator.ResetTrigger("Wallleap");
            _animator.SetBool("JumpAtk", false);
            dashing = false;
            ghost.makeAfterimage = false;
            gravity = baseGravity;
            wallsliding = true;
            wallgrab = false;
            walljumping = false;
            if (_controller.collisionState.left)
            {
                wallDirX = -1;
            }
            else
            if (_controller.collisionState.right)
            {
                wallDirX = 1;
            }
            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * inAirDamping);
            _velocity.y = wallSlideSpeed;
            print("_controller.collisionState.left " + _controller.collisionState.left);
            print("_controller.collisionState.right " + _controller.collisionState.right);
            _animator.SetBool("Wallslide", true);
            
            if (Input.GetButton("Fire1") && (currentDash > 0))
            {

                wallgrab = true;
                canCDdash = false;
                currentDash = (currentDash > 0) ? currentDash - 15f * Time.deltaTime : 0;
                _velocity.x = Mathf.Lerp(_velocity.x, wallDirX * runSpeed, Time.deltaTime * inAirDamping);//grab onto the wall
                if (transform.localScale.x / 2 != wallDirX)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                _velocity.y = 0;
            }
            else
            {
                wallgrab = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") != wallDirX && wallgrab)
                {
                    print("Called");
                    _velocity.x = -wallDirX * wallLeap.x;
                    _velocity.y = wallLeap.y;
                    wallsliding = false;
                    walljumping = false;
                    wallgrab = false;
                    _animator.SetTrigger("Wallleap");
                    soundManager.PlaySound("Jump");
                    soundManager.PlaySound("Dash");
                    ghost.makeAfterimage = true;
                    jumpdashing = true;
                }
                else
                {
                    wallgrab = false;
                    walljumping = true;
                    wallsliding = false;
                    _velocity.y = wallJump.y;
                    _velocity.x = -wallDirX * wallJump.x;
                    _animator.SetTrigger("Walljump");
                    playWalljump();
                }
            }
            if (normalizedHorizontalSpeed == 0)
            {
                _velocity.x = -wallDirX;
                _animator.SetBool("Wallslide", false);
            }
        }
        /*if(wallsliding == true && Input.GetKeyDown(KeyCode.UpArrow))
        {
            wallsliding = false;
            _velocity.y = wallJump.y;
            _velocity.x = -wallDirX * wallJump.x;
            _animator.SetTrigger("Walljump");
        } else*/
        /*if (wallsliding == true && Input.GetButton("Fire1"))
        {
            wallsliding = true;
            wallgrab = true;
            _velocity.x = Mathf.Lerp(_velocity.x, wallDirX * runSpeed, Time.deltaTime * inAirDamping);
            _velocity.y = 0;
            if (Input.GetAxisRaw("Horizontal")!= 0 && Input.GetAxisRaw("Horizontal")!= wallDirX && Input.GetKeyDown(KeyCode.UpArrow))
            {
                wallgrab = false;
                print("Called");
                _velocity.x = -wallDirX * wallLeap.x;
                _velocity.y = wallLeap.y;
                wallsliding = false;
                _animator.SetTrigger("Wallleap");
            }
        }
        
        Delayanimation -= Time.deltaTime;
        if (Delayanimation <= 0)
        {
            Delayanimation = 1f;
            leaping = false;
        }*/
        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control

        if (!wallsliding)
        {
            if (!walljumping)
            {
                var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
                if(currentDash == 0)
                {
                    _animator.SetBool("Dash", false);
                }else
                if (Input.GetButton("Fire1") && currentDash > 0 && dashTime >= 0 && (_controller.isGrounded || dashing))
                {
                    ghost.makeAfterimage = true;
                    dashing = true;
                    canCDdash = false;
                    currentDash = (currentDash > 0) ? currentDash - 15f * Time.deltaTime : 0;

                    if (!DashBoostcreated)
                    {
                        DashBoostcreated = true;
                        Vector3 temp = new Vector3(transform.position.x + 0.411f * transform.localScale.x, transform.position.y + 0.15f * transform.localScale.y, 0);
                        currentDashBoost = Instantiate(DashBoost, temp, transform.rotation);
                        currentDashBoost.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                        currentDashDust = Instantiate(DashDust, transform.position, transform.rotation);
                        currentDashDust.transform.localScale = currentDashBoost.transform.localScale;
                    }
                    _animator.SetFloat("DashTimer", dashTime);
                    _animator.SetBool("Dash", true);
                    dashTime -= Time.deltaTime;
                    if (Input.GetAxisRaw("Horizontal") == 0)
                    {
                        _velocity.x = transform.localScale.x / 2 * dashSpeed;
                    }
                    else
                    {
                        _velocity.x = Input.GetAxisRaw("Horizontal") * dashSpeed;
                    }
                    //_animator.SetFloat("Speed", dashSpeed);
                }
            }
            else
            {
                var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                _velocity.x = Mathf.Lerp(_velocity.x, wallDirX * runSpeed, Time.deltaTime * smoothedMovementFactor);
            }
            // apply gravity before moving

            _velocity.y += gravity * Time.deltaTime;
            endwallslide();
        }
        // if holding down bump up our movement amount and turn off one way platform detection for a frame.
        // this lets us jump down through one way platforms
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (_controller.isGrounded && onewayplat)
            {
                _velocity.y *= 3f;
                _controller.ignoreOneWayPlatformsThisFrame = true;
            }
            else if (wallsliding)
            {
                _velocity.y = 2 * wallSlideSpeed;
            }
            else if (!_controller.isGrounded)
            {
                _velocity.y += gravity * Time.deltaTime;
            }
        }

        _controller.move(_velocity * Time.deltaTime);
        //_velocity.y = Mathf.Clamp(_velocity.y, -8, Mathf.Infinity);
        // grab our current _velocity to use as a base for all calculations
        if (currentDash < totalDash && !wallgrab && canCDdash)
        {
            if (currentDashCooldownTimer > 0)
            {
                currentDashCooldownTimer -= Time.deltaTime;
            }
            if (currentDashCooldownTimer <= 0)
            {
                float temp = currentDash + 30 * Time.deltaTime;
                if (temp <= 100)
                {
                    currentDash = temp;
                }
                else
                {
                    currentDash = 100;
                }
            }
        }
        if (dashTime <= 0)
        {
            if (_controller.isGrounded)
            {
                dashing = false;
                ghost.makeAfterimage = false;
            }
            canCDdash = true;
            DashBoostcreated = false;
            _animator.SetBool("Dash", false);
            currentDashCooldownTimer = DashCooldownTimer;
            Destroy(currentDashBoost);
            Destroy(currentDashDust, 1f);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (_controller.isGrounded)
            {
                dashing = false;
                ghost.makeAfterimage = false;
            }
            canCDdash = true;
            DashBoostcreated = false;
            dashTime = dashDistance / dashSpeed;
            _animator.SetBool("Dash", false);
            if (currentDash > 0)
            {
                currentDashCooldownTimer = DashCooldownTimer;
            }
            _animator.SetTrigger("Dashcut");
            Destroy(currentDashBoost);

            Destroy(currentDashDust, 1f);
        }
        
    }
    void FixedUpdate()
    {
        if (wallsliding && !wallgrab)
        {
            soundTimer -= Time.fixedDeltaTime;
            if (soundTimer <= 0)
            {
                playWallslide();
                soundTimer = soundTimerset;
            }
        }
    }
    void LateUpdate()
    {

        _velocity = _controller.velocity;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (_controller.isGrounded)
            {
                if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && _animator.GetFloat("Speed") == 0 && _velocity.x == 0)
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("NormAtk", true);
                }
                else if (dashing)
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("DashAtk", true);
                }
                else 
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("RunAtk", true);
                }
            }

            if (!_controller.isGrounded)
            {
                if (!wallsliding)
                {
                    soundManager.PlaySound("NormAtk1");
                    _animator.SetBool("JumpAtk", true);
                    if (_velocity.y <= 0)
                    {
                        gravity = gravity / 30f;
                    }
                }
            }
        }
        if (Input.GetButton("Fire2"))
        {
            if(_animator.GetBool("DashAtk") && !_controller.isGrounded && !wallsliding)
            {
                _animator.SetBool("DashAtk", false);
                soundManager.PlaySound("NormAtk1");
                _animator.SetBool("JumpAtk", true);
                if (_velocity.y <= 0)
                {
                    gravity = gravity / 30f;
                }
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            jumpatk = false;
            _animator.SetBool("JumpAtk", false);
            gravity = baseGravity;
        }

        MoveCalculation();

    }






    void endwallslide()
    {

        wallsliding = false;
        _animator.SetBool("Wallslide", false);
    }
    private void startNormAtk(int i)
    {
        _animator.SetBool("NormAtk", true);
        soundManager.PlaySound("NormAtk" + i);
    }
    private void endNormAtk()
    {

        _animator.SetBool("RunAtk", false);
        _animator.ResetTrigger("Dashcut");
        _animator.SetBool("NormAtk", false);
        _animator.ResetTrigger("Attack");
    }
    private void endRunAtk(){
        _animator.SetBool("RunAtk", false);
        _animator.ResetTrigger("Attack");
    }
    private void normalgravity()
    {
        gravity = gravity * 2;
    }

    private void runSound(int i)
    {
        if (i == 1)
        {
            if (soundManager.SoundEnded("Run"))
            {
                soundManager.PlaySound("Run");
            }
        }
        else if(i == 0)
        {
            soundManager.PlaySound("Run");
        }
    }
    private void dashSound()
    {
        
            soundManager.PlaySound("Dash");
    }
    private void jumpAtkSound()
    {
        if (jumpatk)
        {
            jumpatk = false;
            soundManager.PlaySound("NormAtk1");
        }
    }
    private void dashAtkSound()
    {
        soundManager.PlaySound("NormAtk1");
    }
    private void looped()
    {
        _animator.SetBool("RunAtkLoop", false);
    }
    private void dashAtkEnd()
    {
        _animator.SetBool("DashAtk", false);
        _animator.ResetTrigger("Attack");
    }
    private void playWallslide()
    {
        soundManager.PlaySound("Wallslide");
    }
    private void playWalljump()
    {
        soundManager.PlaySound("Wallclimb");
    }
}
