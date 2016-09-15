using UnityEngine;
using System.Collections;

public class PlayerInfo {

    private uint numOfLoosePackets;
    private bool hasFirewall;

    public PlayerInfo()
    {
        numOfLoosePackets = 0;
    }

    public void IncrementNumOfLoosePackets()
    {
        numOfLoosePackets++;
        UIManager.UpdatePickupDisplay(numOfLoosePackets);
    }

    public uint GetNumOfLoosePackets()
    {
        return numOfLoosePackets;
    }

    //Firewall
    public void GiveFirewall()
    {
        hasFirewall = true;
    }

    public void RemoveFirewall()
    {
        hasFirewall = false;
    }

    public bool HasFirewall()
    {
        return hasFirewall;
    }
}
