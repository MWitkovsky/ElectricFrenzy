using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo {

    private uint numOfLoosePackets;

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
}
