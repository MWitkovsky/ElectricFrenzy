using UnityEngine;
using System.Collections;

public class WormHeadHitbox : MonoBehaviour {

    private WormMain worm;
    private float damageDelay = 1.0f, damageTimer;

    void Start()
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
            else if (!PlayerManager.IsAttacking() && !worm.IsStunned())
            {
                if (other.CompareTag(TagManager.Player))
                    worm.Attach();
            }
        }
    }
}
