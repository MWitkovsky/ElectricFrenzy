using UnityEngine;
using System.Collections.Generic;

public class PlayerInfo {

    private List<KeyloggerMain> keyloggers = new List<KeyloggerMain>();
    private List<WormMain> worms = new List<WormMain>();
    private uint numOfLoosePackets;
    private bool hasFirewall;
    private bool hasProxy;
    private bool isFrenzying;

    public PlayerInfo()
    {
        numOfLoosePackets = 0;
    }

    public void IncrementNumOfLoosePackets()
    {
        ++numOfLoosePackets;
        UIManager.UpdatePickupDisplay(numOfLoosePackets);
    }

    public void DecrementNumOfLoosePackets()
    {
        if(numOfLoosePackets > 0)
            --numOfLoosePackets;
        UIManager.UpdatePickupDisplay(numOfLoosePackets);
    }

    public uint GetNumOfLoosePackets()
    {
        return numOfLoosePackets;
    }

    //Keyloggers
    public void AttachKeylogger(KeyloggerMain keylogger)
    {
        keyloggers.Add(keylogger);
    }

    public void DetachKeyloggers()
    {
        foreach (KeyloggerMain k in keyloggers)
        {
            k.Detach();
        }
        keyloggers.Clear();
    }

    //Worms
    public void AttachWorm(WormMain worm)
    {
        worms.Add(worm);
    }

    public void DetachWorms()
    {
        foreach (WormMain w in worms)
        {
            w.Detach();
        }
        worms.Clear();
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

    //Proxy
    public void GiveProxy()
    {
        hasProxy = true;
    }

    public void RemoveProxy()
    {
        hasProxy = false;
    }

    public bool HasProxy()
    {
        return hasProxy;
    }

    //Frenzy
    public void BeginFrenzy()
    {
        isFrenzying = true;
    }

    public void EndFrenzy()
    {
        isFrenzying = false;
    }

    public bool IsFrenzying()
    {
        return isFrenzying;
    }
}
