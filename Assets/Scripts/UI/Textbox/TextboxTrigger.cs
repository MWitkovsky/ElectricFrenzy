using UnityEngine;
using System.Collections;

public class TextboxTrigger : MonoBehaviour {

    [SerializeField] private string message;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            UIManager.OpenTextbox(message);
            Destroy(gameObject);
        }
    }
}
