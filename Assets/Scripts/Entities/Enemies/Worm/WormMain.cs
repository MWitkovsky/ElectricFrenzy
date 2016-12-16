using UnityEngine;
using System.Collections;

public class WormMain : MonoBehaviour {

    [SerializeField]
    private int health, packetYield, numOfSections, maxChaseDistance;
    [SerializeField]
    private float moveSpeed, spottedMoveSpeed, rotateSpeed, spottedRotateSpeed, wallDetectionDistance, turnBeginThresholdDistance;
    [SerializeField]
    private float snakeSpeed, snakeRotateSpeed, stealDelay, hitstunTime, spotDelayTime;
    [SerializeField]
    private AudioClip attachSound, hitSound, detectSound;    

    private GameObject head, body, packet;
    private AudioSource source;
    private Transform target;
    private float stealTimer, hitstunTimer, spotDelayTimer;
    private bool turningFromWall;

    public enum State { idle, spotted, attached, running };
    private State state;

	void Awake () {
	    foreach (Transform t in transform)
        {
            if (t.gameObject.name == "Head")
                head = t.gameObject;
            else if (t.gameObject.name == "Body")
                body = t.gameObject;
        }

        for (int i=0; i<numOfSections; i++)
        {
            GameObject newSection = (GameObject)Instantiate(Resources.Load(ResourcePaths.SnakeBodySectionPrefab), 
                body.transform.position + new Vector3(1.0f * (i + 1), 0.0f, 0.0f), 
                body.transform.rotation);
            newSection.transform.parent = body.transform;
        }

        source = GetComponent<AudioSource>();

        Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward);
        head.transform.forward = dir.normalized;

        state = State.idle;
        stealTimer = stealDelay;
    }

	void FixedUpdate () {
        ////////////
        //AI LOGIC//
        ////////////
        if (hitstunTimer <= 0.0f)
        {  
            if (state == State.idle || state == State.spotted || state == State.attached || state == State.running){
                ////////////
                //MOVEMENT//
                ////////////
                if (state == State.idle || state == State.running)
                {
                    head.transform.Translate(head.transform.forward * Time.fixedDeltaTime * moveSpeed, Space.World);
                }
                else if (state == State.spotted || state == State.attached)
                {
                    //Navigate towards target
                    Quaternion temp = head.transform.rotation;
                    head.transform.LookAt(target);
                    head.transform.rotation = Quaternion.Lerp(temp, head.transform.rotation, spottedRotateSpeed * Time.fixedDeltaTime);
                    head.transform.Translate(head.transform.forward * Time.fixedDeltaTime * spottedMoveSpeed, Space.World);

                    //If target is far enough away, break pursuit
                    if (target && Vector2.Distance(head.transform.position, target.position) > maxChaseDistance)
                    {
                        if (spotDelayTimer > 0.0f)
                            spotDelayTimer -= Time.fixedDeltaTime;
                        else 
                            SetIdle();
                    }
                        
                }

                HandleWallDetection();

                //Handle snake sections
                Transform lastT = null;
                foreach (Transform t in body.transform)
                {
                    if (!lastT)
                    {
                        t.position = Vector3.Lerp(t.position, head.transform.position - head.transform.forward, Time.fixedDeltaTime * snakeSpeed);
                        t.rotation = Quaternion.Lerp(t.rotation, head.transform.rotation, Time.fixedDeltaTime * snakeRotateSpeed);
                    }
                    else
                    {
                        t.position = Vector3.Lerp(t.position, lastT.position - lastT.transform.forward, Time.fixedDeltaTime * snakeSpeed);
                        t.rotation = Quaternion.Lerp(t.rotation, lastT.rotation, Time.fixedDeltaTime * snakeRotateSpeed);
                    }
                    lastT = t;
                }

                if (GameManager.IsDebugEnabled())
                {
                    if (Input.GetKey(KeyCode.LeftArrow))
                        head.transform.Rotate(Time.fixedDeltaTime * rotateSpeed, 0.0f, 0.0f);
                    else if (Input.GetKey(KeyCode.RightArrow))
                        head.transform.Rotate(Time.fixedDeltaTime * -rotateSpeed, 0.0f, 0.0f);
                }

                //////////
                //DAMAGE//
                //////////
                if(state == State.attached)
                {
                    if(stealTimer > 0.0f)
                    {
                        stealTimer -= Time.fixedDeltaTime;
                        spottedRotateSpeed += Time.fixedDeltaTime;
                    }
                    else
                    {
                        Transform temp = null;
                        WormMain newHead = null;
                        WormPacket packet = null;
                        Vector3 tempPosition = Vector3.zero;
                        bool packetAvailable = false;

                        PlayerManager.Damage(10.0f, true);
                        for(int i=0; i<=numOfSections; i++)
                        {
                            packetAvailable = PlayerManager.DecrementNumOfLoosePackets();

                            //Make the fleeing worm head
                            temp = target;
                            newHead = ((GameObject)Instantiate(Resources.Load(ResourcePaths.SnakeHeadOnlyPrefab),
                               temp.position,
                               temp.rotation)).GetComponent<WormMain>();
                            tempPosition = newHead.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0.0f);
                            newHead.GetHead().transform.LookAt(tempPosition);
                            newHead.GetHead().transform.position = tempPosition;
                            newHead.SetRunning();

                            //Make a packet follow it if one was available
                            if (packetAvailable)
                            {
                                temp = newHead.transform;
                                packet = ((GameObject)Instantiate(Resources.Load(ResourcePaths.SnakePacketPrefab), target.position, target.rotation)).GetComponent<WormPacket>();
                                newHead.SetPacket(packet.gameObject);
                                packet.SetTarget(newHead.GetHead().transform);
                            }
                        }
                        PlayerManager.DetachWorm(this);
                        Destroy(gameObject);
                    } 
                }
            }
        }
        else
        {
            hitstunTimer -= Time.fixedDeltaTime;
        }

        //flips transform in Y direction if passes extreme angles on X rotation
        if (Vector3.Dot(head.transform.forward, Vector3.left) > 0)
            head.transform.localScale = new Vector3(head.transform.localScale.x, 1.0f, head.transform.localScale.z);
        else
            head.transform.localScale = new Vector3(head.transform.localScale.x, -1.0f, head.transform.localScale.z);
    }

    //AI for turning from walls
    private void HandleWallDetection()
    {
        RaycastHit2D minHit = Physics2D.Raycast(head.transform.position, head.transform.forward, turnBeginThresholdDistance, LayerMask.GetMask("Walls"));
        RaycastHit2D maxHit = Physics2D.Raycast(head.transform.position, head.transform.forward, wallDetectionDistance, LayerMask.GetMask("Walls"));

        if (maxHit.collider != null)
        {
            if (turningFromWall)
                head.transform.Rotate(Time.fixedDeltaTime * rotateSpeed, 0.0f, 0.0f);
            Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * wallDetectionDistance, Color.red);
        }
        else
        {
            turningFromWall = false;
            Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * wallDetectionDistance, Color.blue);
        }

        if (minHit.collider != null)
        {            
            Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * turnBeginThresholdDistance, Color.red);
            if (state == State.spotted && Vector3.Distance(head.transform.position, minHit.transform.position) > Vector3.Distance(head.transform.position, target.position))
                turningFromWall = false;
            else
                turningFromWall = true;
        }
        else
        {
            Debug.DrawLine(head.transform.position, head.transform.position + head.transform.forward * turnBeginThresholdDistance, Color.yellow);
        }
    }

    public int GetPacketYield()
    {
        return packetYield;
    }

    public void SetPacketYield(int packetYield)
    {
        this.packetYield = packetYield;
    }

    public void SetIdle()
    {
        target = null;
        state = State.idle;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = State.spotted;
        //source.PlayOneShot(detectSound);
    }

    public void Attach()
    {
        transform.parent = target;
        PlayerManager.AttachWorm(this);
        state = State.attached;
        source.PlayOneShot(attachSound);
    }

    public void Detach()
    {
        transform.parent = null;
        hitstunTimer = hitstunTime;
        state = State.spotted;
    }

    public void TakeDamage(int damage)
    {
        Instantiate(Resources.Load(ResourcePaths.SmallHitPrefab), transform.GetChild(0).position, Quaternion.identity);
        health -= damage;
        if (health <= 0)
        {
            //Death stuff
            Instantiate(Resources.Load(ResourcePaths.HitBurstPrefab), transform.GetChild(0).position, Quaternion.identity);

            if (packet)
                packet.GetComponent<WormPacket>().FlyToPlayer();

            for(int i=0; i<packetYield; i++)
                Instantiate(Resources.Load(ResourcePaths.ReclaimedPacketPrefab), head.transform.position, Quaternion.identity);

            PlayerManager.PlayKillEnemySound();

            Destroy(gameObject);
        }
        else
        {
            source.PlayOneShot(hitSound);
        }

        PlayerManager.ResetAttackCooldown();
        hitstunTimer = hitstunTime;
    }

    public bool IsStunned()
    {
        return hitstunTimer > 0.0f;
    }

    public bool IsAttached()
    {
        return state == State.attached;
    }

    public void SetRunning()
    {
        state = State.running;
    }

    public bool IsRunnning()
    {
        return state == State.running;
    }

    public GameObject GetHead()
    {
        return head;
    }

    public void SetPacket(GameObject packet)
    {
        this.packet = packet;
    }
}
