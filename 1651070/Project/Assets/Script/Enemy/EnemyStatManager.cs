using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatManager : MonoBehaviour, IPooledObject
{
    // Start is called before the first frame update
    public float currentHP, maxHP;
    SpriteRenderer sprite;
    Animator animator;
    bool Destroyed = false;
    float TimetoInactive = 1.2f;
    public GameObject spawn, startMark, endMark;
    private GameObject gunPoint;
    SoundManager soundManager;
    public float attackRange;
    public bool startMarkReach;
    public float speed;
    public float idleTime;
    private float currentIdleTime;
    private bool Idle;
    private ObjectPooler objectPooler;
    private bool Cooldown;
    public float AttackCD;
    private float currentAttackCD;
    public Vector3 lasthitposition;



    #region Flashing
    public float flashtimer;
    private float currentflashtimer;
    public bool hurt = false;
    bool flashstarted = false;
    bool white = false;
    public bool dead = false;
    #endregion
    public void OnObjectSpawn()
    {
        hurt = false;
        flashstarted = false;
        white = false;
        dead = false;
        startMarkReach = false;
        Cooldown = false;
        objectPooler = ObjectPooler.Instance;
        gunPoint = transform.Find("GunPoint").gameObject;
        GetComponent<Rigidbody2D>().simulated = true;
        soundManager = SoundManager._instance;
        sprite = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        animator.SetBool("Death", false);
        sprite.enabled = true;
        TimetoInactive = 1.2f;
        transform.Find("BodyParts").gameObject.SetActive(false);
        transform.Find("Explosion").gameObject.SetActive(false);
        Idle = false;
        currentIdleTime = Random.RandomRange(idleTime - 0.5f, idleTime + 3f);
        transform.Find("ZsaberEffect").gameObject.SetActive(false);
        gameObject.transform.Find("Hurtbox").gameObject.SetActive(true);
        Destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0 && !Destroyed)
        {
            Death();
        }
        if (Destroyed)
        {
            TimetoInactive -= Time.deltaTime;
            if (TimetoInactive <= 0)
            {
                spawn.SendMessage("EnemyKilled");
                gameObject.SetActive(false);
            }
        }
        if (hurt)
        {
            if (!flashstarted)
            {
                StartCoroutine("Flash");
                flashstarted = true;
            }
            currentflashtimer -= Time.deltaTime;
            if (currentflashtimer <= 0)
            {
                currentflashtimer = flashtimer;
                hurt = false;
                sprite.material.SetFloat("_FlashAmount", 0);
            }
        }
        if (currentHP > 0) { 
        if (!CheckAttack())
        {
            Patrol();
        }
            if (Cooldown)
            {
                if (currentAttackCD <= 0)
                {
                    Cooldown = false;
                }
                else
                {
                    currentAttackCD -= Time.deltaTime;
                }
            }
        }
    }
    void Death()
    {
        animator.SetBool("Death", true);
        transform.Find("Hurtbox").gameObject.SetActive(false);
        transform.Find("ZsaberEffect").gameObject.SetActive(true);
        if(lasthitposition.x == 2)
        {
            transform.Find("ZsaberEffect").localScale = new Vector3(1f,1f,1f);
        }
        else
        {
            transform.Find("ZsaberEffect").localScale = new Vector3(-1f, 1f, 1f);
        }
        GetComponent<Rigidbody2D>().simulated = false;
    }
    public void SlashDeathEffect()
    {
        transform.Find("OtherHalf").gameObject.SetActive(true);
        transform.Find("Blood").gameObject.SetActive(true);
        if (lasthitposition.x == 2)
        {
            transform.Find("Blood").localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.Find("Blood").localScale = new Vector3(-1f, 1f, 1f);
        }

    }
    public void Explode()
    {
        sprite.enabled = false;
        transform.Find("OtherHalf").gameObject.SetActive(false);
        transform.Find("Blood").gameObject.SetActive(false);
        soundManager.PlaySound("EnemyBasicDie");
        transform.Find("Explosion").gameObject.SetActive(true);
        transform.Find("BodyParts").gameObject.SetActive(true);
        Destroyed = true;
    }
    private bool CheckAttack()
    {
        
        RaycastHit2D[] hit = Physics2D.RaycastAll(gunPoint.transform.position, (transform.localScale/2f) * Vector2.left,attackRange);
        Debug.DrawRay(gunPoint.transform.position, attackRange * (transform.localScale / 2f) * Vector2.left);
        if (hit!= null)
        {
            for (int i = 0; i<hit.Length; i++)
            {
                if (hit[i].collider.tag == "Player"){
                    animator.SetBool("Patrol", false);
                    if (!Cooldown)
                    {
                        animator.SetTrigger("Attack");
                    }
                    return true;
                }
            }
        }
        return false;
    }
    private void Patrol()
    {
        if (!startMarkReach)
        {
            if (Vector2.Distance(startMark.transform.position, transform.position) <= 0.3f)
            {
                animator.SetBool("Patrol", false);
                startMarkReach = true;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                Idle = true;
            }
            else if (Idle)
            {
                currentIdleTime -= Time.deltaTime;
                if (currentIdleTime <= 0)
                {
                    currentIdleTime = Random.RandomRange(idleTime - 0.5f, idleTime + 3f);
                    Idle = false;
                }
            }
            else
            {
                animator.SetBool("Patrol", true);
                if (transform.localScale.x / 2 * speed * Time.deltaTime < Vector2.Distance(startMark.transform.position, transform.position))
                {
                    transform.Translate(transform.localScale.x / 2 * Vector2.left * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.Distance(startMark.transform.position, transform.position) * transform.localScale.x / 2 * Vector2.left);
                }
            }
        }
        else
        {
            if (Vector2.Distance(endMark.transform.position, transform.position) <= 0.3f)
            {
                animator.SetBool("Patrol", false);
                startMarkReach = false;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                Idle = true;
            }
            else if (Idle)
            {
                currentIdleTime -= Time.deltaTime;
                if (currentIdleTime <= 0)
                {
                    currentIdleTime = Random.RandomRange(idleTime - 0.5f, idleTime + 3f);
                    Idle = false;
                }
            }
            else
            {
                animator.SetBool("Patrol", true);
                if (transform.localScale.x / 2 * speed * Time.deltaTime < Vector2.Distance(endMark.transform.position, transform.position))
                {
                    transform.Translate(transform.localScale.x / 2 * Vector2.left * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector2.Distance(endMark.transform.position, transform.position) * transform.localScale.x / 2 * Vector2.left);
                }
            }
        }
    }
    private void Attack()
    {
        var objectPooler = ObjectPooler.Instance;
        GameObject Projectile = objectPooler.SpawnFromPool("ProjectileBasic", gunPoint.transform.position, transform.rotation);
        Projectile.transform.localScale = transform.localScale;
        soundManager.PlaySound("Shoot");
    }
    private void EndAttack()
    {
        Cooldown = true;
        currentAttackCD = AttackCD;
        animator.ResetTrigger("Attack");
    }
    public IEnumerator Flash()
    {
        while (hurt)
        {
            if (!white)
            {
                sprite.material.SetFloat("_FlashAmount", 0.8f);
                white = true;
            }
            else
            {
                white = false;
                sprite.material.SetFloat("_FlashAmount", 0);
            }
            yield return null;
        }
        flashstarted = false;
    }
}
