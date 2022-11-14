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
    public bool ignorePlayer;
    public float AtkPower;
    private SoundManager soundManager;
    // Update is called once per frame
    public void OnObjectSpawn()
    {
        moving = true;
        Invoke("DestroyProjectile", lifeTime);
        soundManager = SoundManager._instance;
        if (soundManager == null)
        {
            Debug.LogError("No SoundManager found in Scene!!!!!");
        }
       
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
        if ((!ignorePlayer && other.CompareTag("Player")) || other.CompareTag("Enemy") )
        {
            
            if(other.CompareTag("Enemy")){
                // other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().lasthitposition = gameObject.transform.parent.localScale;
                other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().hurt = true;
                other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP -= Damage();
                Debug.Log(other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP );
                soundManager.PlaySound("ShootHit");
                if(other.gameObject.transform.parent.gameObject.GetComponent<EnemyStatManager>().currentHP > 0){
                    DestroyProjectile();
                }
            }
            if(other.CompareTag("Player")){
                DestroyProjectile();
            }

        }
    }
    float Damage(){
        return AtkPower;
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
