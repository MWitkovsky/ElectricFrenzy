using UnityEngine;
using System.Collections;

public class LoosePacketHandler : MonoBehaviour {

    [SerializeField]
    private float healAmount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            PlayerManager.IncrementNumOfLoosePackets();
            PlayerManager.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
