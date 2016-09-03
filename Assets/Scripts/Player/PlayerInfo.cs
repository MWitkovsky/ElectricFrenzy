using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerInfo {

    private uint numOfPickups;

    public PlayerInfo()
    {
        numOfPickups = 0;
    }

    public void IncrementNumOfPickups()
    {
        numOfPickups++;
        UIManager.UpdatePickupDisplay(numOfPickups);
    }

    public uint GetNumOfPickups()
    {
        return numOfPickups;
    }
}
