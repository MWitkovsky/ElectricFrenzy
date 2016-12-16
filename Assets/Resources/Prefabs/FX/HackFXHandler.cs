using UnityEngine;
using System.Collections;

public class HackFXHandler : MonoBehaviour
{
    private ParticleSystem ps;
    private float force = 15.0f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Vector3 normal;
        if (GameManager.IsGameActive())
            normal = PlayerManager.GetPlayer().GetComponent<Rigidbody2D>().velocity.normalized;
        else
            normal = Vector3.zero;

        ParticleSystem.MinMaxCurve rateX = new ParticleSystem.MinMaxCurve();
        ParticleSystem.MinMaxCurve rateY = new ParticleSystem.MinMaxCurve();

        ParticleSystem.VelocityOverLifetimeModule vot = ps.velocityOverLifetime;
        rateX.constantMax = -normal.x * force;
        rateY.constantMax = -normal.y * force;

        vot.x = rateX;
        vot.y = rateY;
    }

    void Update()
    {
        if (!ps.IsAlive())
            Destroy(gameObject);
    }
}
