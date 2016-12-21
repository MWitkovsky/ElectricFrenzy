using UnityEngine;
using System.Collections;

public class SakuraMain : MonoBehaviour {

    [SerializeField] private GameObject fallingLeafAttack;
    [SerializeField] private ShootLeaf shootLeafAttack;
    [SerializeField] private RotateLeafAttack rotateLeafAttack;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip destructionSound;
    [SerializeField] private AudioClip windSound;

    
    private Transform player;
    private BossHealthBar healthBar;
    private Vector3 spawnCenter, fallingLeafSpawnPoint, teleportTarget;
    private float spawnCenterHalfX, spawnCenterHalfY;
    private AudioSource source;

    private float packetSpawnDelay, shortPacketSpawnDelay, windDelay, attackDelay, winDelay;
    private float packetSpawnTimer, windDelayTimer, attackDelayTimer;
    private int packetsToSpawn, timesHit;
    private bool windBlow, dead;

	void Start () {
        StartCoroutine(LateStart(0.1f));

        spawnCenterHalfX = 15.0f;
        spawnCenterHalfY = 6.0f;
        spawnCenter = transform.position;
        spawnCenter.x += 2.0f;
        spawnCenter.y += 10.0f;

        fallingLeafSpawnPoint = new Vector3(-60.0f, 20.0f, 0.0f);

        packetSpawnDelay = 0.05f;
        shortPacketSpawnDelay = 0.025f;
        windDelay = 1.0f;
        winDelay = 2.0f;

        source = GetComponent<AudioSource>();
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        healthBar = UIManager.GetBossHealthBar();
        healthBar.gameObject.SetActive(true);
        player = PlayerManager.GetPlayer().transform;
    }

    void Update () {
        if (attackDelayTimer > 0.0f && packetsToSpawn == 0)
        {
            attackDelayTimer -= Time.deltaTime;
        }        
        else if(packetsToSpawn == 0)
        {
            Attack();
            attackDelayTimer = attackDelay;
        }
            
        if (packetSpawnTimer > 0.0f)
            packetSpawnTimer -= Time.deltaTime;

        if (!dead)
        {
            if (packetsToSpawn > 0 && packetSpawnTimer <= 0.0f)
            {
                SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, Resources.Load(ResourcePaths.ReclaimedPacketPrefab), false);
                packetSpawnTimer = packetSpawnDelay;
                --packetsToSpawn;
            }
        }
        else
        {
            if (packetsToSpawn > 160 && packetSpawnTimer <= 0.0f)
            {
                SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, Resources.Load(ResourcePaths.ReclaimedPacketPrefab), false);
                packetSpawnTimer = packetSpawnDelay;
                --packetsToSpawn;
            }
            else if (packetsToSpawn > 0 && packetSpawnTimer <= 0.0f) {
                SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, Resources.Load(ResourcePaths.ReclaimedPacketPrefab), true);
                packetSpawnTimer = shortPacketSpawnDelay;
                --packetsToSpawn;
            }

            if (winDelay > 0.0f && packetsToSpawn == 0)
                winDelay -= Time.deltaTime;
            else if (packetsToSpawn == 0)
                GameManager.BeginVictoryScreen();
        }
        

        if(windDelayTimer > 0.0f)
        {
            windDelayTimer -= Time.deltaTime;
        }
        else if(windBlow)
        {
            source.clip = windSound;
            source.Play();
            PlayerManager.SetTeleporting(true);
            teleportTarget = Physics2D.Raycast(player.position, Vector3.left, 1000.0f, LayerMask.GetMask("Walls")).point;
            windBlow = false;
        }
	}

    void FixedUpdate()
    {
        if (PlayerManager.IsTeleporting())
        {
            Vector3 nextMove = (teleportTarget - player.position).normalized * Time.fixedDeltaTime * 50.0f;

            if (Vector3.Distance(player.position, teleportTarget) > Vector3.Distance((player.position + nextMove), teleportTarget))
            {
                player.Translate(nextMove);
            }
            else
            {
                player.position = teleportTarget;
                PlayerManager.SetTeleporting(false);
            }
        }
    }

    private void Attack()
    {
        GameObject attack = null;
        switch (timesHit)
        {
            case 0:
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, shootLeafAttack.gameObject, false);
                attack.GetComponent<ShootLeaf>().Setup(10, 0.2f, 5.0f);
                attackDelay = 4.0f;
                break;
            case 1:
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, rotateLeafAttack.gameObject, false);
                attack.GetComponent<RotateLeafAttack>().Setup(20, 10.0f, 10.0f);
                attackDelay = 2.0f;
                break;
            case 2:
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, shootLeafAttack.gameObject, false);
                attack.GetComponent<ShootLeaf>().Setup(20, 0.1f, 10.0f);
                attackDelay = 2.0f;
                break;
            case 3:
                Instantiate(fallingLeafAttack, fallingLeafSpawnPoint, Quaternion.identity);
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, rotateLeafAttack.gameObject, false);
                attack.GetComponent<RotateLeafAttack>().Setup(30, 10.0f, 50.0f);
                attackDelay = 6.0f;
                break;
            case 4:
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, rotateLeafAttack.gameObject, false);
                attack.GetComponent<RotateLeafAttack>().Setup(10, 10.0f, 50.0f);
                attackDelay = 1.0f;
                break;
            case 5:
                Instantiate(fallingLeafAttack, fallingLeafSpawnPoint, Quaternion.identity);
                attack = SpawnRandomRegion(spawnCenter, spawnCenterHalfX, spawnCenterHalfY, shootLeafAttack.gameObject, false);
                attack.GetComponent<ShootLeaf>().Setup(40, 0.025f, 15.0f);
                attackDelay = 4.0f;
                break;
        }
    }

    private GameObject SpawnRandomRegion(Vector3 center, float halfX, float halfY, Object objectToInstantiate, bool bigExplosion)
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = center.x + Random.Range(-halfX, halfX);
        spawnPosition.y = center.y + Random.Range(-halfY, halfY);
        GameObject toReturn = Instantiate(objectToInstantiate, spawnPosition, transform.rotation) as GameObject;

        //special for tree
        if(bigExplosion)
            Instantiate(Resources.Load(ResourcePaths.HitBurstPrefab), spawnPosition, transform.rotation);
        else
            Instantiate(Resources.Load(ResourcePaths.SmallHitPrefab), spawnPosition, transform.rotation);
        return toReturn;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagManager.Player))
        {
            if (PlayerManager.IsAttacking())
            {
                source.clip = damageSound;
                source.Play();
                ++timesHit;
                healthBar.ApplyDamage(16.7f);

                if(timesHit != 6)
                {
                    windDelayTimer = windDelay;
                    windBlow = true;
                    packetsToSpawn = 40;
                    packetSpawnTimer = packetSpawnDelay;
                }
                else
                {
                    source.clip = destructionSound;
                    source.Play();
                    packetsToSpawn = 200;
                    packetSpawnTimer = packetSpawnDelay;
                    dead = true;
                }
            }
        }
    }
}
