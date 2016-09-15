using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static PlayerInfo playerInfo;
    private static PlayerController playerController;

    private static GameObject firewall;
    private static MeshRenderer meshRenderer;
    private static float invincibilityTime = 2.0f, amountOfBlinks = 5.0f, blinkTime = 0.1f, blinkIntervalTime;
    private static float invincibilityTimer, blinkIntervalTimer, blinkTimer;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        meshRenderer = GetComponent<MeshRenderer>();
        foreach (Transform t in transform)
        {
            if (t.name == "Firewall")
                firewall = t.gameObject;
        }

        firewall.SetActive(false);

        blinkIntervalTime = invincibilityTime / amountOfBlinks;
    }

    void Update()
    {
        if (invincibilityTimer > 0.0f)
            HandleInvincibility();
    }

    private void HandleInvincibility()
    {
        if (blinkIntervalTimer <= 0.0f)
        {
            meshRenderer.enabled = false;
            blinkTimer = blinkTime;
            blinkIntervalTimer = blinkIntervalTime;
        }

        if (blinkTimer <= 0.0f)
            meshRenderer.enabled = true;

        blinkTimer -= Time.deltaTime;
        blinkIntervalTimer -= Time.deltaTime;
        invincibilityTimer -= Time.deltaTime;

        //ensures player ends up visible
        if (invincibilityTimer <= 0.0f)
            meshRenderer.enabled = true;
    }

    //Grabbing the PlayerInfo from the GameManager
    public static void SetPlayerInfo(PlayerInfo playerInfo)
    {
        PlayerManager.playerInfo = playerInfo;
    }

    public static void BeginInvincibility()
    {
        invincibilityTimer = invincibilityTime;
    }

    //Interfaces with PlayerInfo
    public static void IncrementNumOfLoosePackets()
    {
        playerInfo.IncrementNumOfLoosePackets();
    }

    public static void GiveFirewall()
    {
        playerInfo.GiveFirewall();
        firewall.SetActive(true);
    }

    public static void RemoveFirewall()
    {
        playerInfo.RemoveFirewall();
        firewall.SetActive(false);
    }

    public static bool HasFirewall()
    {
        return playerInfo.HasFirewall();
    }

    //These are interfaces with the UI manager since the health and frenzy bars work a little wonky
    //It makes more sense organizationally to access them from a player manager rather than a UI element
    public static float GetHealth()
    {
       return UIManager.GetHealth();
    }

    public static float GetFrenzyCharge()
    {
        return UIManager.GetFrenzyCharge();
    }

	public static void Heal(float amount)
    {
        UIManager.Heal(amount);
    }

    public static void Damage(float amount, bool fromEnemy)
    {
        if (!fromEnemy)
        {
            UIManager.Damage(amount);
        }
        else
        {
            if(invincibilityTimer <= 0.0f)
            {
                if (playerInfo.HasFirewall())
                    RemoveFirewall();
                else
                    UIManager.Damage(amount);

                playerController.TakeHit();
            }
        }
    }

    public static void AddFrenzyCharge(float amount)
    {
        UIManager.AddFrenzyCharge(amount);
    }
}
