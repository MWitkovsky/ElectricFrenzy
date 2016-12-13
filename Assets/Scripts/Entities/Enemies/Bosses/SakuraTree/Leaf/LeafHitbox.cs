using UnityEngine;
using System.Collections;

public class LeafHitbox : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player) && PlayerManager.CanBeHit())
        {
            PlayerManager.Damage(10.0f, true);
            Destroy(gameObject);
        }
        else if (other.CompareTag(TagManager.Wall))
        {
            Destroy(gameObject);
        }
    }
}
