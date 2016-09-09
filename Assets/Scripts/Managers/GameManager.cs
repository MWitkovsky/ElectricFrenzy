using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static PlayerInfo playerInfo;

    private static UIManager ui;
    private static bool gameActive;
    private static bool debug;

    void Start () {
        //load the player data if exists
        playerInfo = new PlayerInfo();
        PlayerManager.SetPlayerInfo(playerInfo);
        ui = FindObjectOfType<UIManager>();

        gameActive = true;
        debug = true;
	}

    public static bool IsGameActive()
    {
        return gameActive;
    }

    public static void SetGameActive(bool gameActive)
    {
        GameManager.gameActive = gameActive;
    }

    public static bool IsDebugEnabled()
    {
        return debug;
    }

    public static void SetDebugMode(bool debug)
    {
        GameManager.debug = debug;
    }
}
