using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour, IPooledObject
{
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask Ignore;
    private bool moving = true;
    // Update is called once per frame
    public void OnObjectSpawn()
    {
        moving = true;
        Invoke("DestroyProjectile", lifeTime);
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.localScale.x / 2 * Vector2.right, distance, Ignore);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                GetComponent<Animator>().SetBool("GroundHit", true);
            }
        }
        if(moving)
        {
            transform.Translate(transform.localScale.x / 2 * Vector2.left * speed * Time.deltaTime);
        }
    }
    void DestroyProjectile()
    {
        GetComponent<Animator>().SetBool("GroundHit", false);
        gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DestroyProjectile();
        }
    }
    void OnCollisionEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            moving = false;
            GetComponent<Animator>().SetBool("GroundHit", true);
        }
    }
}
