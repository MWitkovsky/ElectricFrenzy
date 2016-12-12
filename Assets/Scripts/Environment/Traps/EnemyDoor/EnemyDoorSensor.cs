using UnityEngine;
using System.Collections;

public class EnemyDoorSensor : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            transform.parent.GetComponent<EnemyDoor>().LockDoor();
            Destroy(gameObject);
        }
    }
}
