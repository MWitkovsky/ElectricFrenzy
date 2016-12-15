using UnityEngine;
using System.Collections;

public class LeafHitbox : MonoBehaviour {

    [SerializeField] private float damage;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player) && PlayerManager.CanBeHit())
        {
            PlayerManager.Damage(damage, true);
            Destroy(gameObject);
        }
        else if (other.CompareTag(TagManager.Wall))
        {
            Destroy(gameObject);
        }
    }
}
