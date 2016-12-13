using UnityEngine;
using System.Collections;

public class PlayerAnimEvents : MonoBehaviour {

    private PlayerController playerController;
    private Animator anim;

    void Start () {
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }
	
    void FinishAttack()
    {
        playerController.FinishAttack();
    }

	void FinishTurn()
    {
        PlayerManager.SetFacingRight(!PlayerManager.IsFacingRight());
    }
}
