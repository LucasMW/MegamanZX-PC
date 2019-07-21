using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeBox : MonoBehaviour
{
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
            if (!other.CompareTag("Player") && !other.CompareTag("Ground") &&!dead)
            {
                if (other.CompareTag("BasicProjectile"))
                    gameObject.transform.parent.GetComponent<PlayerStatManager>().Dodge();
            }
    }
}
