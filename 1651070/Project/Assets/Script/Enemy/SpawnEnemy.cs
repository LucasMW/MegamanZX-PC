using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    private ObjectPooler objectPooler;
    GameObject currentEnemy;
    public string Spawning;
    public int enemyCount = 0;
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount < 1){ 
            GameObject currentEnemy = objectPooler.SpawnFromPool(Spawning, transform.position, transform.rotation);  //Instantiate(AfterImage, transform.position, transform.rotation);
            currentEnemy.transform.localScale = new Vector3(2, 2, 1);
            enemyCount++;
        }
    }
    void EnemyKilled()
    {
        enemyCount--;
    }
}
