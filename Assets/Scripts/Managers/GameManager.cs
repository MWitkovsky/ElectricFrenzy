using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    private static PlayerInfo playerInfo;

    private static SplashScreenHandler splashScreen;
    private static PauseScreen pauseScreen;
    private static VictoryScreen victoryScreen;
    private static MusicHandler musicHandler;
    private static bool gameActive;
    private static bool debug;

    void Start () {
        //load the player data if exists and passes it to the PlayerManager
        playerInfo = new PlayerInfo();
        PlayerManager.SetPlayerInfo(playerInfo);

        splashScreen = FindObjectOfType<SplashScreenHandler>();
        pauseScreen = FindObjectOfType<PauseScreen>();
        victoryScreen = FindObjectOfType<VictoryScreen>();
        musicHandler = FindObjectOfType<MusicHandler>();

        if(pauseScreen)
            pauseScreen.SetActive(false);
        if(victoryScreen)
            victoryScreen.gameObject.SetActive(false);

        gameActive = true;
        debug = false;
	}

    void Update()
    {
        if (debug && Input.GetKeyDown(KeyCode.U))
            TogglePauseGame();
    }

    public static void BeginVictoryScreen()
    {
        victoryScreen.Begin();
    }

    public static void TogglePauseGame()
    {
        if (IsGamePausable())
        {
            Time.timeScale = Time.timeScale > 0.0f ? 0.0f : 1.0f;
            if(Time.timeScale == 0.0f)
                pauseScreen.SetActive(true);
            else
                pauseScreen.SetActive(false);
        }
    }

    public static bool IsGamePausable()
    {
        if (splashScreen.gameObject.activeSelf)
            return false;
        else
            return true;
    }

    public static bool IsGamePaused()
    {
        return pauseScreen.gameObject.activeSelf;
    }

    public static bool IsGameActive()
    {
        return gameActive;
    }

    public static PauseScreen GetPauseScreen()
    {
        return pauseScreen;
    }

    public static MusicHandler GetMusicHandler()
    {
        return musicHandler;
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

    public static void ReturnToMainMenu()
    {
        //TODO: stub
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

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Ends the application
    //Why the hell do we need this again?
    //Now that we made a controller driven game, it makes sense
    public static void ExitGame()
    {
        Application.Quit();
    }
}
