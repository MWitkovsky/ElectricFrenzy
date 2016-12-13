using UnityEngine;
using System.Collections;

public class AdwareDetector : MonoBehaviour {

    private AdwareMain adware;

    void Start()
    {
        adware = transform.parent.parent.GetComponent<AdwareMain>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            adware.SetTarget(other.transform);
        }
    }
}
