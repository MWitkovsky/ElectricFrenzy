using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static PlayerInfo playerInfo;

    private static UIManager ui;
    private static bool gameActive;
    private static bool debug;

    void Start () {
        //load the player data if exists and passes it to the PlayerManager
        playerInfo = new PlayerInfo();
        PlayerManager.SetPlayerInfo(playerInfo);
        ui = FindObjectOfType<UIManager>();

        gameActive = true;
        debug = true;
	}

    void Update()
    {
        if (debug && Input.GetKeyDown(KeyCode.U))
            TogglePauseGame();
    }

    public static void TogglePauseGame()
    {
        Time.timeScale = Time.timeScale > 0.0f ? 0.0f : 1.0f;
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

    public static void ChangeScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public static void NextLevel()
    {
        if(SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) == null)
        {
            //Loop back around
            SceneManager.LoadScene(0);
        }
        //Move to next scene if available, otherwise
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
