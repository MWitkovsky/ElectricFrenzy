using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static Text pickupDisplay;
    private static Timer timerDisplay;
    private static float timer;

    void Start()
    {
        pickupDisplay = GameObject.Find("PickupDisplay").GetComponent<Text>();
        timerDisplay = FindObjectOfType<Timer>();
    }

    void Update()
    {
        if (GameManager.IsGameActive())
        {
            timer += Time.deltaTime;
            timerDisplay.UpdateTimeDisplay(timer);
        }
    }

	public static void UpdatePickupDisplay(uint numOfPickups)
    {
        if (numOfPickups < 10)
            pickupDisplay.text = "00" + numOfPickups;
        else if (numOfPickups < 100)
            pickupDisplay.text = "0" + numOfPickups;
        else
            pickupDisplay.text = numOfPickups.ToString();
    }

}
