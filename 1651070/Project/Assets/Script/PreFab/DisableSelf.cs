using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Disable()
    {
        gameObject.SetActive(false);
    }
}
