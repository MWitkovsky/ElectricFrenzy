using UnityEngine;
using System.Collections;

public class DummyEnemyHandler : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            PlayerManager.Damage(20.0f, true);
        }
    }
}
