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
    public GameObject spawn;


    SoundManager soundManager;
    public void OnObjectSpawn()
    {
        soundManager = SoundManager._instance;
        spawn = GameObject.FindGameObjectWithTag("Spawn Point");
        sprite = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        animator.SetBool("Death", false);
        sprite.enabled = true;
        TimetoInactive = 1.2f;
        transform.Find("BodyParts").gameObject.SetActive(false);
        transform.Find("Explosion").gameObject.SetActive(false);

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
    }
    void Death()
    {
        animator.SetBool("Death", true);
        transform.Find("Hurtbox").gameObject.SetActive(false);
        transform.Find("ZsaberEffect").gameObject.SetActive(true);
    }
    public void SlashDeathEffect()
    {
        transform.Find("OtherHalf").gameObject.SetActive(true);
        transform.Find("Blood").gameObject.SetActive(true);
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
}
