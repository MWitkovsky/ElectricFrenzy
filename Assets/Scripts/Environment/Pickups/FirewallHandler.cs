using UnityEngine;
using System.Collections;

public class FirewallHandler : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player) && !PlayerManager.HasFirewall())
        {
            PlayerManager.GiveFirewall();
            Destroy(gameObject);
        }
    }
}
