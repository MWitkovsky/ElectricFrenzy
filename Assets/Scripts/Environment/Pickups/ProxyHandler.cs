using UnityEngine;
using System.Collections;

public class ProxyHandler : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player) && !PlayerManager.HasProxy())
        {
            PlayerManager.GiveProxy(10.0f);
            Destroy(gameObject);
        }
    }
}
