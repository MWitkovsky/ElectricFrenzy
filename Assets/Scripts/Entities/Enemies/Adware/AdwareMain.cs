using UnityEngine;
using System.Collections;

public class AdwareMain : MonoBehaviour {

    [SerializeField] private int health, packetYield, maxChaseDistance;
    [SerializeField] private float damageYield, moveSpeed, chaseSpeed, attackDelay, hitstunTime, spotDelayTime;

    private Transform target, enemy, home, idleTarget;
    private float stealTimer, hitstunTimer, attackDelayTimer, spotDelayTimer;
    private State state;

    public enum State { idle, spotted };

    void Start () {
	    foreach(Transform t in transform)
        {
            if(t.name == "Adware(Enemy)")
            {
                enemy = t;
            }
            else if(t.name == "Home")
            {
                home = t;
                idleTarget = t.GetChild(0);
            }
        }
	}

    void FixedUpdate()
    {
        ////////////
        //AI LOGIC//
        ////////////
        if (hitstunTimer <= 0.0f)
        {
            if (state == State.idle)
            {
                enemy.position = Vector3.Lerp(enemy.position, idleTarget.position, moveSpeed * Time.fixedDeltaTime);
            }
            else if (state == State.spotted && CanAttack())
            {
                enemy.Translate((target.position - enemy.position).normalized * Time.fixedDeltaTime * chaseSpeed, Space.World);

                //If target is far enough away, break pursuit
                if (target && Vector2.Distance(home.position, target.position) > maxChaseDistance)
                {
                    if (spotDelayTimer > 0.0f)
                        spotDelayTimer -= Time.fixedDeltaTime;
                    else
                        SetIdle();
                }
            }
        }
        else
        {
            hitstunTimer -= Time.fixedDeltaTime;
        }

        if (attackDelayTimer > 0.0f)
        {
            attackDelayTimer -= Time.fixedDeltaTime;

            if(attackDelayTimer <= 0.0f)
            {
                if (target && Vector2.Distance(home.position, target.position) > maxChaseDistance)
                    SetIdle();
                else
                    SetTarget(target);
            }
        }
            
    }

    public void TakeDamage(int damage)
    {
        Instantiate(Resources.Load(ResourcePaths.SmallHitPrefab), transform.GetChild(0).position, Quaternion.identity);
        health -= damage;

        if (health <= 0)
        {
            //Death stuff
            for (int i = 0; i < packetYield; i++)
                Instantiate(Resources.Load(ResourcePaths.ReclaimedPacketPrefab), enemy.position, Quaternion.identity);

            Destroy(gameObject);
        }

        PlayerManager.ResetAttackCooldown();
        hitstunTimer = hitstunTime;
    }

    public void SetIdle()
    {
        state = State.idle;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = State.spotted;
        spotDelayTimer = spotDelayTime;
    }

    public void HitPlayer()
    {
        home.position = enemy.position;
        attackDelayTimer = attackDelay;
    }

    public bool CanAttack()
    {
        return attackDelayTimer <= 0.0f;
    }

    public bool IsStunned()
    {
        return hitstunTimer > 0.0f;
    }

    public int GetPacketYield()
    {
        return packetYield;
    }

    public float GetDamageYield()
    {
        return damageYield;
    }
}
