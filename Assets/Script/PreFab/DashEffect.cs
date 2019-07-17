using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;
public class DashEffect : MonoBehaviour
{
    private CharacterController2D _controller;
    public GameObject DashBoost;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && _controller.isGrounded)
        {
            Vector3 dashBoosttrans = new Vector3(transform.position.x + 0.369f, transform.position.y + 0.165f, 0);
            GameObject currentDashBoost = Instantiate(DashBoost, dashBoosttrans, transform.rotation);
            Destroy(currentDashBoost, 1f);
        }
    }
}
