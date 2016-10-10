    using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    //Main player oversight tools
    private static GameObject player;
    private static PlayerInfo playerInfo;
    private static PlayerController playerController;

    //Firewall tools
    private static GameObject firewallPrefab, firewall;

    //Proxy tools
    private static AfterimageGenerator afterimages;
    private static SkinnedMeshRenderer meshRenderer;

    //Taking damage and invincibility
    private static float invincibilityTime = 2.0f, amountOfBlinks = 5.0f, blinkTime = 0.1f, blinkIntervalTime;
    private static float invincibilityTimer, blinkIntervalTimer, blinkTimer;

    void Start()
    {
        player = GameObject.Find("Player");
        playerController = FindObjectOfType<PlayerController>();
        afterimages = FindObjectOfType<AfterimageGenerator>();

        firewallPrefab = (GameObject)Resources.Load(ResourcePaths.FirewallPrefab);
        meshRenderer = GameObject.Find("PlugMesh_002").GetComponent<SkinnedMeshRenderer>();

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

    //Interfaces with PlayerController
    public static bool IsAttacking()
    {
        return playerController.IsAttacking();
    }

    public static bool IsFacingRight()
    {
        return playerController.IsFacingRight();
    }

    public static void SetFacingRight(bool facingRight)
    {
        playerController.SetFacingRight(facingRight);
    }

    public static bool IsTeleporting()
    {
        return playerController.IsTeleporting();
    }

    public static void SetTeleporting(bool isTeleporting)
    {
        playerController.SetTeleporting(isTeleporting);
    }

    public static Transform GetModelTransform()
    {
        return playerController.GetModelTransform();
    }

    //Interfaces with PlayerInfo
    public static void IncrementNumOfLoosePackets()
    {
        playerInfo.IncrementNumOfLoosePackets();
    }

    //Keylogger
    public static bool StealPacket()
    {
        if (playerInfo.GetNumOfLoosePackets() > 0)
        {
            playerInfo.DecrementNumOfLoosePackets();
            return true;
        }
        else
        {
            Damage(1.0f, false);
            return false;
        }
    }

    public static void AttachKeylogger(KeyloggerMain keylogger)
    {
        playerInfo.AttachKeylogger(keylogger);
    }

    public static void DetachKeyloggers()
    {
        playerInfo.DetachKeyloggers();
    }

    //Firewall
    public static void GiveFirewall()
    {
        playerInfo.GiveFirewall();
        firewall = (GameObject)Instantiate(firewallPrefab, player.transform.position, player.transform.rotation);
        firewall.transform.parent = player.transform;
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

    //Proxy
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

    //Frenzy
    public static bool IsFrenzying()
    {
        return playerInfo.IsFrenzying();
    }

    public static void BeginFrenzy()
    {
        playerInfo.BeginFrenzy();
    }

    public static void EndFrenzy()
    {
        playerInfo.EndFrenzy();
    }

    //These are interfaces with the UI manager since the health and frenzy bars work a little wonky
    //It makes more sense organizationally to access them from a player manager rather than a UI element
    public static float GetHealth()
    {
       return UIManager.GetHealth();
    }

	public static void Heal(float amount)
    {
        UIManager.Heal(amount);
    }

    public static void Damage(float amount, bool knockback)
    {
        if (!knockback)
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

    public static float GetFrenzyCharge()
    {
        return UIManager.GetFrenzyCharge();
    }

    public static void AddFrenzyCharge(float amount)
    {
        UIManager.AddFrenzyCharge(amount);
    }

    public static void RemoveFrenzyCharge(float amount)
    {
        UIManager.RemoveFrenzyCharge(amount);
    }
}
