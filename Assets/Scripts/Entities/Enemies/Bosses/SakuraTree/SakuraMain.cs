using UnityEngine;
using System.Collections;

public class SakuraMain : MonoBehaviour {

    private BossHealthBar healthBar;

	void Start () {
        healthBar = UIManager.GetBossHealthBar();
        healthBar.gameObject.SetActive(true);
    }
	
	void Update () {
	}

    private void SpawnRandomRegion(Vector3 center, float halfX, float halfY, Object objectToInstantiate, int amount)
    {
        Vector3 spawnPosition = Vector3.zero;
        while(amount > 0)
        {
            spawnPosition.x = center.x + Random.Range(-halfX, halfX);
            spawnPosition.y = center.y + Random.Range(-halfY, halfY);
            Instantiate(objectToInstantiate, spawnPosition, transform.rotation);
            --amount;
        }
    }
}
