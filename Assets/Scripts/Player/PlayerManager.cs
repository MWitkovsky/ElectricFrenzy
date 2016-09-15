using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static GameObject player;
    private static PlayerInfo playerInfo;
    private static PlayerController playerController;

    private static GameObject firewallPrefab, firewall;
    private static AfterimageGenerator afterimages;
    private static SkinnedMeshRenderer meshRenderer;
    private static float invincibilityTime = 2.0f, amountOfBlinks = 5.0f, blinkTime = 0.1f, blinkIntervalTime;
    private static float invincibilityTimer, blinkIntervalTimer, blinkTimer;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = FindObjectOfType<PlayerController>();
        afterimages = FindObjectOfType<AfterimageGenerator>();

        firewallPrefab = (GameObject)Resources.Load(ResourcePaths.FirewallPrefab);
        meshRenderer = GetComponent<SkinnedMeshRenderer>();

        afterimages.enabled = false;

        blinkIntervalTime = invincibilityTime / amountOfBlinks;
    }

    void Update()
    {
        if (invincibilityTimer > 0.0f)
            HandleInvincibility();

        if (GameManager.IsDebugEnabled())
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!HasProxy())
                    GiveProxy();
                else
                    RemoveProxy();
            }
        }
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
        firewall = (GameObject)Instantiate(firewallPrefab, player.transform.position, player.transform.rotation);
        firewall.transform.parent = player.transform;
        firewall.transform.Translate(new Vector3(0.0f, 0.0f, -2.0f));
    }

    public static void RemoveFirewall()
    {
        playerInfo.RemoveFirewall();
        if (firewall)
            Destroy(firewall);
    }

    public static bool HasFirewall()
    {
        return playerInfo.HasFirewall();
    }

    public static void GiveProxy()
    {
        playerInfo.GiveProxy();
        afterimages.enabled = true;
    }

    public static void RemoveProxy()
    {
        playerInfo.RemoveProxy();
        afterimages.DestroyAllAfterimages();
        afterimages.enabled = false;
    }

    public static bool HasProxy()
    {
        return playerInfo.HasProxy();
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
