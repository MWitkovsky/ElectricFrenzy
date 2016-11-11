using UnityEngine;
using System.Collections;

public class WormHeadHitbox : MonoBehaviour {

    private WormMain worm;
    private float damageDelay = 0.75f, damageTimer;

    void Awake()
    {
        worm = transform.parent.parent.GetComponent<WormMain>();
    }

    void Update()
    {
        if (damageTimer > 0.0f)
            damageTimer -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            if (PlayerManager.IsAttacking() && !worm.IsAttached() && !worm.IsStunned() && damageTimer <= 0.0f)
            {
                if (PlayerManager.IsFrenzying())
                    worm.TakeDamage(3);
                else
                    worm.TakeDamage(1);

                damageTimer = damageDelay;
            }
            else if (!PlayerManager.IsAttacking() && !worm.IsStunned() && !worm.IsRunnning())
            {
                if (other.CompareTag(TagManager.Player))
                    worm.Attach();
            }
        }
    }
}
