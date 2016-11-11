using UnityEngine;
using System.Collections;

public class WormDetector : MonoBehaviour {

    private WormMain worm;

    void Awake()
    {
        worm = transform.parent.parent.gameObject.GetComponent<WormMain>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player) && !worm.IsRunnning())
        {
            worm.SetTarget(other.transform);
        }
    }
}
