using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttack : MonoBehaviour
{
    void Start()
    {

    }
    // Start is called before the first frame update
    public void End()
    {
        this.gameObject.GetComponent<Animator>().SetInteger("Attacktype", 0);
        gameObject.GetComponent<DamageCalculation>().first = true;
    }
}
