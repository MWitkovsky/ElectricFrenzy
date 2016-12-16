using UnityEngine;
using System.Collections;

public class KeyloggerMain : MonoBehaviour {

    [SerializeField] private int health, packetYield;
    [SerializeField] private float moveSpeed, chaseSpeed, rotateSpeed, stealDelay, hitstunTime, wallDetectDistance;
    [SerializeField] private AudioClip attachSound, hitSound, deadSound, detectSound;

    private AudioSource source;
    private Transform target;
    private State state;
    private float stealTimer, hitstunTimer;
    private bool facingRight;

    public enum State { idle, turning, spotted, attached };

    void Awake()
    {
        source = GetComponent<AudioSource>();
        state = State.idle;
        stealTimer = stealDelay;
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
                RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, transform.forward, wallDetectDistance, LayerMask.GetMask("Walls"));
                if (wallCheck.collider != null)
                    state = State.turning;    

                transform.Translate(transform.forward * Time.fixedDeltaTime * moveSpeed, Space.World);
            }
            else if(state == State.turning)
            {
                if (facingRight)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, 270.0f, rotateSpeed * Time.fixedDeltaTime), transform.eulerAngles.z);
                    if(Mathf.Abs(transform.eulerAngles.y - 270.0f) < 0.5f)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 270.0f, transform.eulerAngles.z);
                        facingRight = false;
                        state = State.idle;
                    }
                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, 90.0f, rotateSpeed * Time.fixedDeltaTime), transform.eulerAngles.z);
                    if (Mathf.Abs(transform.eulerAngles.y - 90.0f) < 0.5f)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90.0f, transform.eulerAngles.z);
                        facingRight = true;
                        state = State.idle;
                    }
                }

                transform.Translate(transform.forward * Time.fixedDeltaTime * moveSpeed, Space.World);
            }
            else if (state == State.spotted)
            {
                transform.LookAt(target);
                transform.Translate(transform.forward * Time.fixedDeltaTime * chaseSpeed, Space.World);
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
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = State.spotted;
        source.PlayOneShot(detectSound);
    }

    public void Attach()
    {
        transform.parent = target;
        GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerManager.AttachKeylogger(this);
        state = State.attached;
        source.PlayOneShot(attachSound);
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
        Instantiate(Resources.Load(ResourcePaths.HitBurstPrefab), transform.position, Quaternion.identity);
        health -= damage;
        if(health <= 0)
        {
            //Death stuff
            for (int i = 0; i < packetYield; i++)
                Instantiate(Resources.Load(ResourcePaths.ReclaimedPacketPrefab), transform.position, Quaternion.identity);

            source.PlayOneShot(deadSound);

            Destroy(gameObject);
        }
        else
        {
            source.PlayOneShot(hitSound);
        }

        PlayerManager.ResetAttackCooldown();
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

    public int GetPacketYield()
    {
        return packetYield;
    }

    public void SetPacketYield(int packetYield)
    {
        this.packetYield = packetYield;
    }
}
