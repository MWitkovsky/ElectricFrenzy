using UnityEngine;
using System.Collections;

public class HackFXHandler : MonoBehaviour
{
    private ParticleSystem ps;
    private float forceX = 15.0f;
    private float forceY = 15.0f;
    private PlayerInputHandler pih;
    // Use this for initialization
    void Start()
    {
        pih = GameObject.Find("Player").GetComponent<PlayerInputHandler>();
        ps = GetComponent<ParticleSystem>();
    }

    //Moving particle system with Plug's direction
    void Update()
    {
        if (pih.GetMovementVec().x > 0) //Moving Right
            forceX = -15.0f;

        if (pih.GetMovementVec().x < 0) //Moving Left
            forceX = 15.0f;

        if (pih.GetMovementVec().y > 0) //Moving Up
            forceY = -15.0f;

        if (pih.GetMovementVec().y < 0) //Moving Down
            forceY = 15.0f;


        ParticleSystem.MinMaxCurve rateX = new ParticleSystem.MinMaxCurve();
        ParticleSystem.MinMaxCurve rateY = new ParticleSystem.MinMaxCurve();

        ParticleSystem.VelocityOverLifetimeModule vot = ps.velocityOverLifetime;
        rateX.constantMax = forceX;
        rateY.constantMax = forceY;

        vot.x = rateX;
        //vot.y = rateY;
        if (!ps.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}
