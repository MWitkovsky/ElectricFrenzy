using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static Text pickupDisplay;
    private static Timer timerDisplay;
    private static HealthBar healthBar;
    private static FrenzyBar frenzyBar;
    private static float timer;

    void Start()
    {
        pickupDisplay = GameObject.Find("LoosePacketDisplay").GetComponent<Text>();
        timerDisplay = FindObjectOfType<Timer>();
        healthBar = FindObjectOfType<HealthBar>();
        frenzyBar = FindObjectOfType<FrenzyBar>();
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

    //Interfaces with the health and frenzy bars
    public static float GetHealth()
    {
        return healthBar.GetHealth();
    }

    public static float GetFrenzyCharge()
    {
        return frenzyBar.GetCharge();
    }

    public static void Heal(float amount)
    {
        healthBar.Heal(amount);
    }

    public static void Damage(float amount)
    {
        healthBar.ApplyDamage(amount);
    }

    public static void AddFrenzyCharge(float amount)
    {
        frenzyBar.AddFrenzyCharge(amount);
    }
}
