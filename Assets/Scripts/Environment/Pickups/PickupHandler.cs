using UnityEngine;
using System.Collections;

public class PickupHandler : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            GameManager.playerInfo.IncrementNumOfPickups();
            Destroy(gameObject);
        }
    }
}
