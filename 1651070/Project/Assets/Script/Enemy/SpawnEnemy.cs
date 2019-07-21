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
    public int MaxEnemy;
    private bool goleft = true;
    public float spawnDelay;
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        for (int i = 0; i< MaxEnemy; i++)
        {
            GameObject currentEnemy = objectPooler.SpawnFromPool(Spawning, transform.position, transform.rotation);
            if (goleft)
            {
                goleft = false;
                currentEnemy.transform.localScale = new Vector3(2, 2, 1);
                currentEnemy.GetComponent<EnemyStatManager>().spawn = gameObject;
                currentEnemy.GetComponent<EnemyStatManager>().startMark = gameObject.transform.Find("StartMark").gameObject;
                currentEnemy.GetComponent<EnemyStatManager>().endMark = gameObject.transform.Find("EndMark").gameObject;
            }
            else
            {
                goleft = true;
                currentEnemy.transform.localScale = new Vector3(-2, 2, 1);
                currentEnemy.GetComponent<EnemyStatManager>().spawn = gameObject;
                currentEnemy.GetComponent<EnemyStatManager>().startMark = gameObject.transform.Find("EndMark").gameObject;
                currentEnemy.GetComponent<EnemyStatManager>().endMark = gameObject.transform.Find("StartMark").gameObject;
            }
            enemyCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount < MaxEnemy)
        {
            StartCoroutine(Spawn());
        }
    }
    void EnemyKilled()
    {
        enemyCount--;
    }
    IEnumerator Spawn()
    {
        enemyCount++;
        yield return new WaitForSeconds(spawnDelay);
        GameObject currentEnemy = objectPooler.SpawnFromPool(Spawning, transform.position, transform.rotation);
        if (goleft)
        {
            goleft = false;
            currentEnemy.transform.localScale = new Vector3(2, 2, 1);
            currentEnemy.GetComponent<EnemyStatManager>().spawn = gameObject;
            currentEnemy.GetComponent<EnemyStatManager>().startMark = gameObject.transform.Find("StartMark").gameObject;
            currentEnemy.GetComponent<EnemyStatManager>().endMark = gameObject.transform.Find("EndMark").gameObject;
        }
        else
        {
            goleft = true;
            currentEnemy.transform.localScale = new Vector3(-2, 2, 1);
            currentEnemy.GetComponent<EnemyStatManager>().spawn = gameObject;
            currentEnemy.GetComponent<EnemyStatManager>().startMark = gameObject.transform.Find("EndMark").gameObject;
            currentEnemy.GetComponent<EnemyStatManager>().endMark = gameObject.transform.Find("StartMark").gameObject;
        }
    }
}
