using UnityEngine;
using System.Collections;

public class KeyloggerHitbox : MonoBehaviour {

    private KeyloggerMain keylogger;
    private float damageDelay = 0.2f, damageTimer;

	void Start () {
        keylogger = transform.parent.GetComponent<KeyloggerMain>();
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
            if (PlayerManager.IsAttacking() && !keylogger.IsAttached() && damageTimer <= 0.0f)
            {
                if (PlayerManager.IsFrenzying())
                    keylogger.TakeDamage(3);
                else
                    keylogger.TakeDamage(1);

                damageTimer = damageDelay;
            }
            else
            {
                if (other.CompareTag(TagManager.Player))
                    keylogger.Attach();
            }
        }
    }
}
