using UnityEngine;
using System.Collections;

public class DDOSMain : MonoBehaviour {

    [SerializeField] private float fuseTime, damageYield;

    private Transform player;
    private float explosionRadius, fuseTimer;

	void Start () {
        player = GameObject.Find("Player").transform;
        fuseTimer = fuseTime;
        explosionRadius = GetComponent<CircleCollider2D>().radius;
	}
	
	void FixedUpdate() {
        fuseTimer -= Time.fixedDeltaTime;
        if(fuseTimer <= 0.0f)
        {
            if(Vector3.Distance(transform.position, player.position) < explosionRadius)
                PlayerManager.Damage(damageYield, true);
            //Spawn explosion graphic and play explosion sound
            Destroy(gameObject);
        }
	}
}
