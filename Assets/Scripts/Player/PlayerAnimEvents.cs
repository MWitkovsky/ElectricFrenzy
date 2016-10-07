using UnityEngine;
using System.Collections;

public class PlayerAnimEvents : MonoBehaviour {

    private Animator anim;

    void Start () {
        anim = GetComponent<Animator>();
    }
	

	void FinishTurn()
    {
        PlayerManager.SetFacingRight(!PlayerManager.IsFacingRight());
    }
}
