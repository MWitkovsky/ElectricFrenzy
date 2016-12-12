using UnityEngine;
using System.Collections.Generic;

public class PlayerInfo {

    private List<KeyloggerMain> keyloggers = new List<KeyloggerMain>();
    private List<WormMain> worms = new List<WormMain>();
    private uint numOfLoosePackets;
    private bool hasFirewall;
    private bool hasProxy;
    private bool isFrenzying;

    //Status ailments
    private Status status;
    public enum Status { OK, Poison, Paralyzed, Slowed };

    public PlayerInfo()
    {
        numOfLoosePackets = 0;
        status = Status.OK;
    }

    public void IncrementNumOfLoosePackets()
    {
        ++numOfLoosePackets;
        UIManager.UpdatePickupDisplay(numOfLoosePackets);
    }

    public bool DecrementNumOfLoosePackets()
    {
        if (numOfLoosePackets > 0)
        {
            --numOfLoosePackets;
            UIManager.UpdatePickupDisplay(numOfLoosePackets);
            return true;
        }

        return false;
    }

    public uint GetNumOfLoosePackets()
    {
        return numOfLoosePackets;
    }

    //Status
    public Status GetStatus()
    {
        return status;
    }

    public void SetStatus(Status status)
    {
        this.status = status;
    }

    public void CureStatus()
    {
        status = Status.OK;
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

    public void DetachWorm(WormMain worm)
    {
        worms.Remove(worm);
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
