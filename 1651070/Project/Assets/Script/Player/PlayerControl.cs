﻿using System.Collections;
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
    public GameObject hitBox;
    public GameObject hurtBox;
    public GameObject dodgeBox;
    public bool inviframestarted = false;
    public float inviframeduration;
    public float inviframeoffset;
    private float currentinviframe;
    private float countframe;
    float countframe2;

    float shotHolding = 0.0f;
    void Awake()
    {
        currentinviframe = inviframeduration;
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
    }


    void onTriggerExitEvent(Collider2D col)
    {
    }

    #endregion


    void Start()
    {
        soundManager = SoundManager._instance;
        if (soundManager == null)
        {
            Debug.LogError("No SoundManager found in Scene!!!!!");
        }
        _animator.SetBool("Shoot",false);
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
            if (hitBox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpAtk - Hitbox"))
            {
                hitBox.GetComponent<Animator>().Play("Hitbox Base");
            }
            _animator.ResetTrigger("Jump");
            _animator.ResetTrigger("Wallleap");
            if (dashing == false)
            {
                InviframeOff();
                inviframestarted = false;
                jumpdashing = false;
            }

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                ghost.makeAfterimage = false;
                _animator.ResetTrigger("Attack");
                _animator.SetBool("JumpAtk", false);
                _animator.SetBool("DashAtk", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
            {
                ghost.makeAfterimage = false;
                _animator.ResetTrigger("Attack");
                _animator.SetBool("JumpAtk", false);
                _animator.SetBool("DashAtk", false);
                _animator.SetBool("RunAtk", false);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                ghost.makeAfterimage = false;
                _animator.SetBool("JumpAtk", false);
                _animator.SetBool("DashAtk", false);
                _animator.SetBool("RunAtk", false);
            }
        }
        if (!_controller.isGrounded)
        {
            InviframeOff();
            inviframestarted = false;
            _animator.SetBool("On Ground", false);
            _animator.SetBool("RunAtk", false);
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
                if (_controller.isGrounded)
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
            _animator.SetBool("RunAtk", false);
            _animator.ResetTrigger("Attack");
            if (dashing)
            {
                InviframeOff();
                inviframestarted = false;
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

            if (hitBox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpAtk - Hitbox"))
            {
                hitBox.GetComponent<Animator>().Play("Hitbox Base");
            }
            dashing = false;
            InviframeOff();
            inviframestarted = false;
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
                if (Input.GetButton("Fire1") && currentDash > 0 && dashTime >= 0 && (_controller.isGrounded || dashing) && !_animator.GetBool("NormAtk"))
                {
                    if(!inviframestarted)
                    {
                        inviframestarted = true;
                        countframe = Time.time + inviframeoffset;
                        countframe2 = countframe + inviframeduration;
                        
                    }else
                    {
                        if(Time.time >= countframe)
                        {
                            InviframeOn();
                            countframe = Mathf.Infinity;
                        }else
                        if (Time.time >= countframe2)
                        {
                            InviframeOff();
                        }
                    }
                    ghost.makeAfterimage = true;
                    dashing = true;
                    canCDdash = false;
                    currentDash = (currentDash > 0) ? currentDash - 20f * Time.deltaTime : 0;

                    if (!DashBoostcreated)
                    {
                        DashBoostcreated = true;
                        Vector3 temp = new Vector3(transform.position.x + 0.411f * transform.localScale.x, transform.position.y + 0.15f * transform.localScale.y, 0);
                        currentDashBoost = Instantiate(DashBoost, temp, transform.rotation);
                        currentDashBoost.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                        currentDashDust = Instantiate(DashDust, transform.position, transform.rotation);
                        currentDashDust.transform.localScale = currentDashBoost.transform.localScale;
                    }
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
                InviframeOff();
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
                InviframeOff();
                
                inviframestarted = false;
            }
            canCDdash = true;
            DashBoostcreated = false;
            dashTime = dashDistance / dashSpeed;
            _animator.SetBool("Dash", false);
            if (currentDash > 0)
            {
                currentDashCooldownTimer = DashCooldownTimer;
            }
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
                if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && _animator.GetFloat("Speed") == 0 && Mathf.Abs(Mathf.RoundToInt(_velocity.x)) == 0)
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("NormAtk", true);
                }
                else if (dashing)
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("DashAtk", true);
                }
                else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    _animator.SetTrigger("Attack");
                    _animator.SetBool("RunAtk", true);
                }
            }

            if (!_controller.isGrounded)
            {
                if (!wallsliding && !wallgrab && !walljumping)
                {
                    hitBox.GetComponent<Animator>().SetInteger("Attacktype", 4);
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

                _animator.ResetTrigger("Attack");
                hitBox.GetComponent<Animator>().SetInteger("Attacktype", 4);
                _animator.SetBool("DashAtk", false);
                soundManager.PlaySound("NormAtk1");
                _animator.SetBool("JumpAtk", true);
                if (_velocity.y <= 0)
                {
                    gravity = gravity / 30f;
                }
            }else if(_controller.isGrounded && !wallsliding && _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                _animator.SetBool("RunAtk", true);
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            jumpatk = false;
            if (hitBox.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpAtk - Hitbox"))
            {
                hitBox.GetComponent<Animator>().Play("Hitbox Base");
                hitBox.GetComponent<Animator>().SetInteger("Attacktype", 0);
            }
            _animator.SetBool("JumpAtk", false);
            gravity = baseGravity;
        }

        if(Input.GetButtonDown("Fire3")){
            _animator.SetBool("Shoot",true);
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)){
                 _animator.ResetTrigger("idleShoot");
                _animator.Play("RunShoot");
            } else {
                _animator.Play("idleShoot");
                 _animator.ResetTrigger("RunShoot");
            }
            
            Debug.Log("Should shoot!");
            Attack();
        }
        if(Input.GetButton("Fire3")){
            _animator.ResetTrigger("idleShoot");
            _animator.ResetTrigger("RunShoot");
            shotHolding += Time.deltaTime;
            Debug.Log("Charging " + shotHolding);
        }
         if(Input.GetButtonUp("Fire3")){
            _animator.SetBool("Shoot",false);
            
            if(shotHolding > 0.4){
                 Attack();
            } else {
                _animator.ResetTrigger("idleShoot");
                _animator.ResetTrigger("RunShoot");
            }
            shotHolding = 0.0f;

        }


        MoveCalculation();

    }

    bool IsRunning() {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Run") || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow);
    }

    int ShotLevel() {
        if(shotHolding > 0.5){
            if(shotHolding > 1.0) {
                return 2;
            }
            return 1;
        }
        return 0;
    }


     private void Attack()
    {
        _animator.SetBool("Shoot",true);
        if(IsRunning()) {
            _animator.Play("RunShoot");
           
        } else {
            _animator.Play("idleShoot");
        }
        Debug.Log("Shot level " + ShotLevel() );
        if(ShotLevel() == 0){
            soundManager.PlaySound("Shoot");
            Transform aim = transform.Find("Aim");
            var objectPooler = ObjectPooler.Instance;
            Debug.Log(normalizedHorizontalSpeed);
            Quaternion rotation  = normalizedHorizontalSpeed < 0 ? aim.rotation : MirrorOnXAxis(aim.rotation);
            GameObject Projectile = objectPooler.SpawnFromPool("PlayerShot", aim.position, aim.rotation);
            Projectile.transform.localScale = -transform.localScale;
        } else if(ShotLevel() == 1){
            soundManager.PlaySound("Shoot");
            Transform aim = transform.Find("Aim");
            var objectPooler = ObjectPooler.Instance;
            Debug.Log(normalizedHorizontalSpeed);
            Quaternion rotation  = normalizedHorizontalSpeed < 0 ? aim.rotation : MirrorOnXAxis(aim.rotation);
            GameObject Projectile = objectPooler.SpawnFromPool("PlayerShotHalfCharged", aim.position, aim.rotation);
            Projectile.transform.localScale = -transform.localScale;

        } else if (ShotLevel() == 2){
            soundManager.PlaySound("Shoot");
            Transform aim = transform.Find("Aim");
            var objectPooler = ObjectPooler.Instance;
            Debug.Log(normalizedHorizontalSpeed);
            Quaternion rotation  = normalizedHorizontalSpeed < 0 ? aim.rotation : MirrorOnXAxis(aim.rotation);
            GameObject Projectile = objectPooler.SpawnFromPool("PlayerShotFullyCharged", aim.position, aim.rotation);
            Projectile.transform.localScale = -transform.localScale;
        }
        _animator.SetBool("Shoot",false);
        
       // Projectile.transform.rotation = MirrorOnXAxis(Projectile.transform.rotation);
    }

    public static Quaternion MirrorOnXAxis(Quaternion q) 
    {
        //q.x = -q.x;
        //q.y = -q.y;
        q.z = -q.z;
        q.w = -q.w;
        return q;
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
    private void InviframeOn()
    {
        hurtBox.SetActive(false);
        dodgeBox.SetActive(true);
    }
    private void InviframeOff()
    {
        hurtBox.SetActive(true);
        dodgeBox.SetActive(false);
    }
}
