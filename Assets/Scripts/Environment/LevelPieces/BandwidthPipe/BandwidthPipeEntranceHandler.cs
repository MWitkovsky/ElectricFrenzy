using UnityEngine;
using System.Collections;

public class BandwidthPipeEntranceHandler : MonoBehaviour {

    private BandwidthPipeMain main;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            other.gameObject.layer = 0;
            main.SetPlayer(other.transform.parent);
            main.SetHitbox(other.transform);
        }
    }

    public void SetMain(BandwidthPipeMain main)
    {
        this.main = main;
    }
}
