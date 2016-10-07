using UnityEngine;
using System.Collections;

public class PlayerAnimEvents : MonoBehaviour {

    private Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
    }
	

	void FinishTurn()
    {
        bool temp = PlayerManager.IsFacingRight();
        PlayerManager.SetFacingRight(!PlayerManager.IsFacingRight());
        print(temp + " " + PlayerManager.IsFacingRight());
    }
}
