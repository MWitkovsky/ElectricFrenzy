using UnityEngine;
using System.Collections;

public class WormMain : MonoBehaviour {

    [SerializeField]
    private int packetYield, numOfSections;
    [SerializeField]
    private float moveSpeed, spottedMoveSpeed, rotateSpeed, spottedRotateSpeed, wallDetectionDistance, turnBeginThresholdDistance;
    [SerializeField]
    private float snakeSpeed, snakeRotateSpeed, stealDelay, hitstunTime;

    private GameObject head, body;
    private Transform target;
    private int health;
    private float stealTimer, hitstunTimer;
    private bool turningFromWall;

    public enum State { idle, spotted, attached, running };
    private State state;

	void Start () {
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

        Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward);
        head.transform.forward = dir.normalized;
    }

	void FixedUpdate () {
        ////////////
        //MOVEMENT//
        ////////////
        if(state != State.spotted)
        {
            head.transform.Translate(head.transform.forward * Time.fixedDeltaTime * moveSpeed, Space.World);
        }
        else
        {
            Quaternion temp = head.transform.rotation;
            head.transform.LookAt(target);
            head.transform.rotation = Quaternion.Lerp(temp, head.transform.rotation, spottedRotateSpeed * Time.fixedDeltaTime);
            head.transform.Translate(head.transform.forward * Time.fixedDeltaTime * spottedMoveSpeed, Space.World);
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

    public void SetTarget(Transform target)
    {
        this.target = target;
        state = State.spotted;
    }

    public void Attach()
    {
        transform.parent = target;
        GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerManager.AttachWorm(this);
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
        if (health <= 0)
        {
            //Death stuff
            Destroy(gameObject);
        }

        hitstunTimer = hitstunTime;
    }

    public bool IsStunned()
    {
        return hitstunTimer > 0.0f;
    }
}
