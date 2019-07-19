﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
public class DamageCalculation : MonoBehaviour
{
    // Start is called before the first frame update
    int counter = 1;
    float reset = 0.25f;
    public HashSet<GameObject> check = new HashSet<GameObject>();
    private PlayerControl playerControl;
    private float baseGravity;
    public bool first = true;
    private CharacterController2D characterController;
    private SoundManager soundManager;
    //LayerMask HurtBoxMask;
    void Awake()
    {
        soundManager = SoundManager._instance;
        //HurtBoxMask = LayerMask.GetMask("HurtBox");
        playerControl = gameObject.transform.parent.GetComponent<PlayerControl>();
        characterController = gameObject.transform.parent.GetComponent<CharacterController2D>();
        baseGravity = playerControl.gravity;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!check.Contains(other.gameObject))
        {
            soundManager.PlaySound("ZsaberHit");
            Debug.Log("Hit number " + counter);
            counter++;
            check.Add(other.gameObject);
            other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (!check.Contains(other.gameObject))
        {
            Debug.Log("Hit number " + counter);
            counter++;
            check.Add(other.gameObject);

            soundManager.PlaySound("ZsaberHit");
            other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
        }
    }
    void Update()
    {
        if (characterController.isGrounded)
        {
            first = true;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        #region JumpATK
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpAtk - Hitbox"))
        {
            if (first)
            {
                playerControl.gravity /= 2;
                first = false;
            }
            if (check.Contains(other.gameObject))
            {
                reset -= Time.deltaTime;
                if (reset <= 0)
                {
                    Debug.Log("Hit number " + counter);
                    counter++;

                    soundManager.PlaySound("ZsaberHit");
                    other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
                    reset = 0.25f;
                }
            }
            else
            {
                check.Add(other.gameObject); 
                Debug.Log("Hit number " + counter);
                counter++;

                soundManager.PlaySound("ZsaberHit");
                other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
            }
        }
        #endregion
        else
        #region NormalAtk
        {
            
            if (!check.Contains(other.gameObject))
            {
                check.Add(other.gameObject);
                    Debug.Log("Hit number " + counter);
                    counter++;

                soundManager.PlaySound("ZsaberHit");
                other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
            }
        }
        #endregion
    }
    int Damage()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("JumpAtk - Hitbox"))
        {
            return 10;
        }
        else if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("zx Norm 1st - Hitbox"))
        {
            return 20;
        }
        else if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("zx Norm 2nd - Hitbox"))
        {
            return 30;
        }
        else if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("zx Norm 3rd - Hitbox")) return 50;
        else if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DashAtk - Hitbox")) return 50;
        else if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("RunAtkZsaber - Hitbox")) return 15;
        return 0;
    }
}