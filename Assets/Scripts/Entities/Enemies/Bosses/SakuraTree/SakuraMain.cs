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
}
