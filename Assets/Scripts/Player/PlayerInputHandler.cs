using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {

    private PlayerController playerController;
    private Vector2 move;
    private bool attack;
    private bool dash;

	void Start () {
        playerController = FindObjectOfType<PlayerController>();
	}
	
    void Update()
    {
        attack = Input.GetButtonDown("Fire1");
        dash = Input.GetButtonDown("Jump");

        if (attack)
            playerController.Attack();
        if (dash)
            playerController.Dash(move);
    }

	void FixedUpdate() {
        move = Vector2.zero;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        move = v * Vector2.up + h * Vector2.right;
        playerController.Move(move);
    }

    //Keyboard controls are digital, getting keys here will avoid analogue emulation
    //Currently unused..
    private void DigitalInput(float h, float v)
    {
        if (Input.GetKey(KeyCode.W))
            v = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            v = -1.0f;
        if (Input.GetKey(KeyCode.A))
            h = -1.0f;
        else if (Input.GetKey(KeyCode.D))
            h = 1.0f;
    }
}
