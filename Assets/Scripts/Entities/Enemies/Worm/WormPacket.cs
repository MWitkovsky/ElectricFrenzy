using UnityEngine;
using System.Collections;

public class WormPacket : MonoBehaviour {

    private Transform target;
    private float lerpSpeed = 5.0f;
    private bool onWorm = true;

	void FixedUpdate () {
        if (target)
        {
            if (onWorm)
            {
                transform.position = Vector3.Lerp(transform.position, target.position - target.forward, lerpSpeed * Time.fixedDeltaTime);
                print(target.position);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed * Time.fixedDeltaTime);

                if (Vector2.Distance(transform.position, target.position) < 0.5f)
                {
                    PlayerManager.IncrementNumOfLoosePackets();
                    PlayerManager.Heal(0.5f);
                    Destroy(gameObject);
                }
            }
        }
	}

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void FlyToPlayer()
    {
        target = PlayerManager.GetPlayer().transform;
        onWorm = false;
    }
}
