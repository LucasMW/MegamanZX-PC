using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class ReloadScene : MonoBehaviour
{
    // Start is called before the first frame update
   
        
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped)
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
