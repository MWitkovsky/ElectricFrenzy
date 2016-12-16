using UnityEngine;
using System.Collections;

public class LoosePacketHandler : MonoBehaviour {

    [SerializeField]
    private float healAmount;

    private Transform target;
    private float lerpSpeed = 17.5f;

    void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.5f)
            {
                PlayerManager.IncrementNumOfLoosePackets();
                PlayerManager.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            target = other.transform;
            //PlayerManager.PlayPacketAttractSound();
        }      
    }
}
