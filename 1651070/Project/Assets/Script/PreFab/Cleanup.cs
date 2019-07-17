using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
