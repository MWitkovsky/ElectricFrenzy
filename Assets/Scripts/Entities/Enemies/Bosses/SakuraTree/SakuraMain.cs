using UnityEngine;
using System.Collections;

public class SakuraMain : MonoBehaviour {

    private BossHealthBar healthBar;
    private Vector3 spawnCenter;
    private float spawnCenterHalfX, spawnCenterHalfY;

    private float packetSpawnDelay;
    private float packetSpawnTimer;
    private int packetsToSpawn;

	void Start () {
        StartCoroutine(LateStart(0.1f));

        spawnCenterHalfX = 15.0f;
        spawnCenterHalfY = 6.0f;
        spawnCenter = transform.position;
        spawnCenter.x += 2.0f;
        spawnCenter.y += 10.0f;

        packetSpawnDelay = 0.05f;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        healthBar = UIManager.GetBossHealthBar();
        healthBar.gameObject.SetActive(true);
    }

    void Update () {
        if (packetSpawnTimer > 0.0f)
            packetSpawnTimer -= Time.deltaTime;

        if(packetsToSpawn > 0 && packetSpawnTimer <= 0.0f)
        {
            SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, Resources.Load(ResourcePaths.ReclaimedPacketPrefab));
            packetSpawnTimer = packetSpawnDelay;
            --packetsToSpawn;
        }
	}

    private void SpawnRandomRegion(Vector3 center, float halfX, float halfY, Object objectToInstantiate)
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = center.x + Random.Range(-halfX, halfX);
        spawnPosition.y = center.y + Random.Range(-halfY, halfY);
        Instantiate(objectToInstantiate, spawnPosition, transform.rotation);

        //special for tree
        Instantiate(Resources.Load(ResourcePaths.SmallHitPrefab), spawnPosition, transform.rotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            if (PlayerManager.IsAttacking())
            {
                healthBar.ApplyDamage(16.7f);
                packetsToSpawn = 40;
                packetSpawnTimer = packetSpawnDelay;
            }
        }
    }
}
