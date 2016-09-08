using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static PlayerInfo playerInfo;

    private static UIManager ui;
    private static bool gameActive;

    void Start () {
        //load the player data if exists
        playerInfo = new PlayerInfo();
        ui = FindObjectOfType<UIManager>();

        gameActive = true;
	}

    public static bool IsGameActive()
    {
        return gameActive;
    }

    public static void SetGameActive(bool gameActive)
    {
        GameManager.gameActive = gameActive;
    }
}
