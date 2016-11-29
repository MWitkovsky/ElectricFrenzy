using UnityEngine;
using System.Collections;

public class DDOSMain : MonoBehaviour {

    [SerializeField] private float fuseTime, damageYield;

    private Transform player;
    private float explosionRadius, fuseTimer;
    private bool set;

	void Start () {
        player = GameObject.Find("Player").transform;
        fuseTimer = fuseTime;
        explosionRadius = GetComponent<CircleCollider2D>().radius;
	}
	
	void FixedUpdate() {
        if (set)
        {
            fuseTimer -= Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.25f, 1.0f - (fuseTimer / fuseTime));
        }

        if(fuseTimer <= 0.0f)
        {
            if(Vector3.Distance(transform.position, player.position) < explosionRadius)
                PlayerManager.Damage(damageYield, true);
            //Spawn explosion graphic and play explosion sound
            Instantiate(Resources.Load(ResourcePaths.HitBurstPrefab), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
            set = true;
    }
}
