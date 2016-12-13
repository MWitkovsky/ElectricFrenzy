﻿using UnityEngine;
using System.Collections;

public class ShootLeaf : MonoBehaviour {

    [SerializeField] private int numOfLeaves;
    [SerializeField] private float startDelay, shotDelay, shotSpeed;

    private LeafMain leaf;
    private float startDelayTimer, shotDelayTimer;

    void Start()
    {
        startDelayTimer = startDelay;
    }

    void FixedUpdate()
    {
        if(startDelayTimer > 0.0f)
        {
            startDelayTimer -= Time.fixedDeltaTime;
        }
        else
        {
            if (shotDelayTimer > 0.0f)
            {
                shotDelayTimer -= Time.fixedDeltaTime;
            }
            else
            {
                leaf = ((GameObject)Instantiate(Resources.Load(ResourcePaths.SakuraLeafPrefab), transform.position, transform.rotation)).GetComponent<LeafMain>();
                leaf.SetShotDirection(PlayerManager.GetPlayer().transform.position - transform.position, shotSpeed);

                if (--numOfLeaves == 0)
                    Destroy(gameObject);

                shotDelayTimer = shotDelay;
            }
        }
    }
}
