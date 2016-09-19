using UnityEngine;
using System.Collections;

public class KeyloggerMain : MonoBehaviour {

    [SerializeField]
    private float speed, packetYield, stealDelay, hitstunTime;

    private Transform target;
    [SerializeField]
    private State state;
    private int health;
    private float stealTimer, hitstunTimer;

    public enum State { idle, spotted, attached };

    void Start()
    {
        health = 3;
        state = State.idle;
        stealTimer = stealDelay;
    }

    void FixedUpdate()
    {
        if (hitstunTimer <= 0.0f)
        {
            if (state == State.idle)
            {
                //Make raycast check for walls here, turn around and patrol at a fixed speed
            }
            else if (state == State.spotted)
            {
                Vector3 dir = target.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward);
                transform.Translate(dir * Time.fixedDeltaTime * speed, Space.World);
            }
            else if (state == State.attached)
            {
                if (stealTimer >= 0.0f)
                {
                    stealTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    if (PlayerManager.StealPacket())
                        ++packetYield;

                    stealTimer = stealDelay;
                }
            }
        }
        else
        {
            hitstunTimer -= Time.fixedDeltaTime;
        }

        //flips transform in Y direction if passes extreme angles on Z rotation
        if (transform.rotation.eulerAngles.z >= 90.0f && transform.rotation.eulerAngles.z <= 270.0f)
            transform.localScale = new Vector3(transform.localScale.x, -0.5f, transform.localScale.z);
        else
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = State.spotted;
    }

    public void Attach()
    {
        transform.parent = target;
        GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerManager.AttachKeylogger(this);
        state = State.attached;
    }

    public void Detach()
    {
        transform.parent = null;
        GetComponent<Rigidbody2D>().isKinematic = false;
        hitstunTimer = hitstunTime;
        state = State.spotted;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            //Death stuff
            Destroy(gameObject);
        }

        hitstunTimer = hitstunTime;
    }

    public bool IsAttached()
    {
        return state == State.attached;
    }

    public bool IsStunned()
    {
        return hitstunTimer > 0.0f;
    }
}
