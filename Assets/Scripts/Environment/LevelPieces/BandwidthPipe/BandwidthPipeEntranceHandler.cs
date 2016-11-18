using UnityEngine;
using System.Collections;

public class BandwidthPipeEntranceHandler : MonoBehaviour {

    private BandwidthPipeMain main;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            main.SetPlayer(other.transform);
            other.gameObject.layer = 0;
        }
    }

    public void SetMain(BandwidthPipeMain main)
    {
        this.main = main;
    }
}
