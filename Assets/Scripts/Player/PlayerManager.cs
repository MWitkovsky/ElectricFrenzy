using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    private static PlayerInfo playerInfo;

    public static void SetPlayerInfo(PlayerInfo playerInfo)
    {
        PlayerManager.playerInfo = playerInfo;
    }

    //Interfaces with PlayerInfo
    public static void IncrementNumOfLoosePackets()
    {
        playerInfo.IncrementNumOfLoosePackets();
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

    public static void Damage(float amount)
    {
        UIManager.Damage(amount);
    }

    public static void AddFrenzyCharge(float amount)
    {
        UIManager.AddFrenzyCharge(amount);
    }
}
