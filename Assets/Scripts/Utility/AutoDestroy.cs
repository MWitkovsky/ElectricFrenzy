using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    private ParticleSystem ps;
    private float force = 15.0f;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!ps.IsAlive())
        {
            Destroy(this.gameObject);
        }
	}
}
