using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float flashtimer;
    private float currentflashtimer;
    private bool hurt = false;
    SpriteRenderer sprite;
    bool flashstarted = false;
    bool white = false;
    public bool dead = false;
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
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
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
            if (currentflashtimer <= 0)
            {
                currentflashtimer = flashtimer;
                hurt = false;
                sprite.material.SetFloat("_FlashAmount", 0);
            }
        }
    }
}
