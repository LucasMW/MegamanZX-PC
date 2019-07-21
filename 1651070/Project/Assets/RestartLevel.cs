using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartLevel : MonoBehaviour
{
    public float TimetoRestart;
    private float currentTimetoRestart;
    private bool restart = false;
    // Start is called before the first frame update
    void Start()
    {
        
        currentTimetoRestart = TimetoRestart;
    }
    void Update()
    {
        if (restart == true)
        {
            currentTimetoRestart -= Time.deltaTime;
            if (currentTimetoRestart <= 0)
                SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
        else
        {
            currentTimetoRestart = TimetoRestart;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            restart = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            restart = false;
        }
    }
}
