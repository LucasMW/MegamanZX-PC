using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadytoExplode : MonoBehaviour
{
    // Start is called before the first frame update
    void ReadyExplode()
    {
        SendMessageUpwards("Explode");
    }
}
