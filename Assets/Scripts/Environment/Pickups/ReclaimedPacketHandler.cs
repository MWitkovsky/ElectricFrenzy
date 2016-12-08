using UnityEngine;
using System.Collections;

public class ReclaimedPacketHandler : MonoBehaviour {

    [SerializeField]
    private float healAmount;

    private Transform player;
    private Vector3 initTarget;
    private float lerpSpeed = 17.5f, initSpeed = 6.0f, delay = 1.0f;
    private bool toPlayer;

    void Start()
    {
        player = PlayerManager.GetPlayer().transform;
        initTarget = transform.position + new Vector3(Random.value, Random.value, Random.value) * (Random.value * 2.0f);
    }

    void FixedUpdate()
    {
        if (toPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, lerpSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, player.position) < 0.5f)
            {
                PlayerManager.IncrementNumOfLoosePackets();
                PlayerManager.Heal(healAmount);
                Destroy(gameObject);
            }
        }
        else
        {
            delay -= Time.fixedDeltaTime;

            if(delay > 0)
            {
                if(Vector3.Distance(transform.position, initTarget) > 0.1f)
                    transform.position += (initTarget - transform.position).normalized * initSpeed * Time.fixedDeltaTime;
            }
            else
            {
                toPlayer = true;
            }
                
        }
    }
}
