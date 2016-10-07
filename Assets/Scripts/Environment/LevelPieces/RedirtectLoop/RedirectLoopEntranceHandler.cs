using UnityEngine;
using System.Collections;

public class RedirectLoopEntranceHandler : MonoBehaviour {

    private RedirectLoopMain main;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
            main.SetPlayer(other.transform);
    }

    public void SetMain(RedirectLoopMain main)
    {
        this.main = main;
    }
}
