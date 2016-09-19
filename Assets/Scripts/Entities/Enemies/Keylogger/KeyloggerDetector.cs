using UnityEngine;
using System.Collections;

public class KeyloggerDetector : MonoBehaviour {

    private KeyloggerMain keylogger;

    void Start()
    {
        keylogger = transform.parent.gameObject.GetComponent<KeyloggerMain>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            keylogger.SetTarget(other.transform);
        }
    }
}
