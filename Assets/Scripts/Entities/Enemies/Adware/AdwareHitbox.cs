using UnityEngine;
using System.Collections;

public class AdwareHitbox : MonoBehaviour {

    private AdwareMain adware;
    private float damageDelay = 0.0f, damageTimer;

    void Start()
    {
        adware = transform.parent.parent.GetComponent<AdwareMain>();
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
            if (PlayerManager.IsAttacking() && damageTimer <= 0.0f)
            {
                adware.TakeDamage(PlayerManager.CalculateDamageDone());

                damageTimer = damageDelay;
            }
            else if (!PlayerManager.IsAttacking() && !adware.IsStunned())
            {
                if (other.CompareTag(TagManager.Player))
                {
                    PlayerManager.Damage(adware.GetDamageYield(), true);
                    adware.HitPlayer();
                }
                    
            }
        }
    }
}
