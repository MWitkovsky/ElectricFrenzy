using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {

    private PlayerController playerController;
    private Vector2 move;

	void Start () {
        playerController = FindObjectOfType<PlayerController>();
	}
	
	void FixedUpdate() {
        move = Vector2.zero;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Keyboard controls are digital, getting keys here will avoid analogue emulation


        move = v * Vector2.up + h * Vector2.right;
        playerController.Move(move);
    }
}
