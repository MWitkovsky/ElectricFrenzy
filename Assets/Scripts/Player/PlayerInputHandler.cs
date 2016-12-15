using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {

    private PlayerController playerController;
    private Vector2 move;
    private bool attack, teleport, frenzy, pause, stickReset;

	void Start () {
        playerController = FindObjectOfType<PlayerController>();
	}
	
    void Update()
    {
        pause = Input.GetButtonDown("Submit");
        attack = Input.GetButtonDown("Fire1");
        teleport = Input.GetButtonDown("Jump");
        frenzy = Input.GetButtonDown("Fire2");

        if (GameManager.IsGamePaused())
        {
            float v = Input.GetAxisRaw("Vertical");
            if (stickReset)
            {
                if (v > 0.5f)
                {
                    GameManager.GetPauseScreen().MoveUpMenu();
                    stickReset = false;
                }
                else if (v < -0.5f)
                {
                    GameManager.GetPauseScreen().MoveDownMenu();
                    stickReset = false;
                }
            }              

            if (Mathf.Abs(v) < 0.1f)
                stickReset = true;

            if (attack)
                GameManager.GetPauseScreen().SelectMenuOption();
        }
        else
        {
            if (attack)
                playerController.Attack(move);
            if (teleport)
                playerController.Teleport(move);
            if (frenzy)
                Frenzy();
        }

        if (pause)
            GameManager.TogglePauseGame();
    }

	void FixedUpdate() {
        if (!GameManager.IsGamePaused())
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            move = v * Vector2.up + h * Vector2.right;
            playerController.Move(move);
        } 
    }

    private void Frenzy()
    {
        if(PlayerManager.GetFrenzyCharge() == 100.0f)
            PlayerManager.BeginFrenzy();
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

    public Vector2 GetMovementVec()
    {
        return move;
    }
}
