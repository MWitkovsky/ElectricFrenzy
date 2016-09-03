using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static Text pickupDisplay;

    void Start()
    {
        pickupDisplay = GameObject.Find("PickupDisplay").GetComponent<Text>();
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
