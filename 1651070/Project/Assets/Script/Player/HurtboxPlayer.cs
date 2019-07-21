using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    SoundManager soundManager;
    public float flashtimer;
    private float currentflashtimer;
    private bool hurt = false;
    SpriteRenderer sprite;
    bool flashstarted = false;
    bool white = false;
    public bool dead = false;
    void Start()
    {
        soundManager = SoundManager._instance;
        currentflashtimer = flashtimer;
        sprite = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hurt)
        {
            if (!flashstarted)
            {
                StartCoroutine("Flash");
                flashstarted = true;
            }
            currentflashtimer -= Time.deltaTime;
            if(currentflashtimer <= 0)
            {
                currentflashtimer = flashtimer;
                hurt = false;
                sprite.material.SetFloat("_FlashAmount", 0);
            }
        }
        if (dead)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.activeInHierarchy == true)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Ground") && !dead)
            {
                if (other.CompareTag("BasicProjectile"))
                {
                    gameObject.transform.parent.GetComponent<PlayerStatManager>().HPchange(-10f);
                    soundManager.PlaySound("ZX Hurt");
                    hurt = true;
                }else if (other.CompareTag("Trap"))
                {
                    gameObject.transform.parent.GetComponent<PlayerStatManager>().HPchange(-100f);
                    soundManager.PlaySound("ZX Hurt");
                    hurt = true;
                }

            }
        }
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
