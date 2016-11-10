using UnityEngine;
using System.Collections;

public class WormDetector : MonoBehaviour {

    private WormMain worm;

    void Start()
    {
        worm = transform.parent.parent.gameObject.GetComponent<WormMain>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            worm.SetTarget(other.transform);
        }
    }
}
