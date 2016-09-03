using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static PlayerInfo playerInfo;

	void Start () {
        //load the player data if exists
        playerInfo = new PlayerInfo();
	}
}
