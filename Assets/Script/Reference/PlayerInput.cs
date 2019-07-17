using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            player.OnJumpInputUp();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.OnFallInput();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            player.OnMeleeInputDown();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            player.OnMeleeInputUp();
        }
    }
}
