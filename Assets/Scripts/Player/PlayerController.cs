using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Basic Movement
    [SerializeField]
    private float moveSpeed = 15.0f, backwardsMoveSpeed = 13.5f, turnDelay, acceleration = 1.0f, turnSpeed = 0.5f;

    //Attack
    [SerializeField]
    private float attackTime = 0.15f, attackCooldown = 0.1f;

    //Teleport
    [SerializeField]
    private float teleportDistance, teleportCooldown, unusedTeleportTravelTime, unusedTeleportStun;

    //Timers
    [SerializeField]
    private float recoilForce, recoilTime, stunTime;

    //Not set in editor
    private Rigidbody2D rb;
    private Vector2 lastAttack;
    private float attackTimer, attackCooldownTimer, teleportCooldownTimer, turnTimer, recoilTimer, stunTimer;
    private bool isRecoiling, isStunned, isTeleporting;

    //animation variables
    private Animator anim;
    private Transform model;
    private Vector2 currentMoveSpeed;
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        foreach (Transform t in transform)
        {
            if (t.name == "Mesh")
            {
                model = t;
                break;
            }
        }
        facingRight = true;

        GetComponent<AfterimageGenerator>().SetModelTransform(model);
    }

    void Update()
    {
        //HANDLE ATTACK
        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if(attackCooldownTimer > 0.0f)
            {
                if (Vector2.Dot(lastAttack, Vector2.left) > 0)
                    SetFacingRight(false);
                else
                    SetFacingRight(true);

                attackCooldownTimer -= Time.deltaTime;
            }
                
        }    

        //HANDLE HIT RECOIL AND STUN
        if (recoilTimer > 0.0f)
        {
            recoilTimer -= Time.deltaTime;
        }
        else if (isRecoiling)
        {
            isRecoiling = false;
            isStunned = true;
            stunTimer = stunTime;
        }
        else if (stunTimer > 0.0f)
        {
            stunTimer -= Time.deltaTime;
        }
        else if (isStunned)
        {
            isStunned = false;
            PlayerManager.BeginInvincibility();
        }

        //TELEPORT COOLDOWN
        if (teleportCooldownTimer > 0.0f)
            teleportCooldownTimer -= Time.deltaTime;

        //TURNING
        if ((turnTimer > 0.0f) && ((facingRight && rb.velocity.x < 0.0f) || (!facingRight && rb.velocity.x > 0.0f)))
        {
            turnTimer -= Time.deltaTime;
            if(turnTimer <= 0.0f)
            {
                anim.SetTrigger("turn");
                turnTimer = turnDelay;
            }
        }
        else
        {
            turnTimer = turnDelay;
        }
            

        //Anim flags
        if (facingRight)
            anim.SetFloat("horizontalMoveSpeed", rb.velocity.x);
        else
            anim.SetFloat("horizontalMoveSpeed", 0.0f-rb.velocity.x);

        anim.SetFloat("verticalMoveSpeed", rb.velocity.y);
    }

    void FixedUpdate()
    {
        if (ShouldLockVelocity())
        {
            //rb.velocity = Vector2.zero;
        }
    }
    int q = 0;
	public void Move(Vector2 move)
    {
        if (AbleToMove())
        {
            float tiltPercent = move.magnitude;

            if (rb.velocity.magnitude > moveSpeed * tiltPercent)
                rb.velocity = rb.velocity.normalized * (moveSpeed * tiltPercent);
            else
                rb.AddForce(move.normalized * acceleration);

            //turning
            float magnitude = rb.velocity.magnitude;
            rb.velocity = Vector2.Lerp(rb.velocity.normalized, move.normalized, turnSpeed);
            rb.velocity *= magnitude;
        }
    }

    public void Attack(Vector2 move)
    {
        if (AbleToMove())
        {
            attackTimer = attackTime;
            attackCooldownTimer = attackCooldown;
            rb.velocity = Vector3.zero;
            rb.AddForce(move.normalized * moveSpeed * 2.0f, ForceMode2D.Impulse);

            lastAttack = move.normalized;
            model.LookAt(transform.position + (Vector3)move.normalized);
            anim.SetTrigger("attack");
        }
    }

    public void Teleport(Vector2 move)
    {
        if (AbleToMove() && teleportCooldownTimer <= 0.0f)
        {
            PlayerManager.DetachEnemies();
            transform.Translate(move.normalized * teleportDistance, Space.World);
            teleportCooldownTimer = teleportCooldown;
        }
    }

    public void TakeHit()
    {
        if (CanTakeHit())
        {
            anim.SetTrigger("damage");
            if (facingRight)
                rb.AddForce(Vector2.left * recoilForce, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.right * recoilForce, ForceMode2D.Impulse);

            recoilTimer = recoilTime;
            isRecoiling = true;
        }
    }

    private bool AbleToMove()
    {
        if (attackCooldownTimer <= 0.0f && !isRecoiling && !isStunned && !isTeleporting)
            return true;
        else
            return false;
    }

    private bool ShouldLockVelocity()
    {
        if (attackTimer <= 0.0f && recoilTimer <= 0.0f)
            return true;
        else
            return false;
    }

    private bool CanTakeHit()
    {
        if (isRecoiling && !isTeleporting)
            return false;
        else
            return true;
    }

    //GETTERS AND SETTERS
    public bool IsFacingRight()
    {
        return facingRight;
    }

    public void SetFacingRight(bool facingRight)
    {
        this.facingRight = facingRight;
        if (facingRight)
            model.LookAt(transform.position + Vector3.right);
        else
            model.LookAt(transform.position + Vector3.left);
    }

    public bool IsAttacking()
    {
        return attackTimer > 0.0f;
    }

    public bool IsTeleporting()
    {
        return isTeleporting;
    }

    public void SetTeleporting(bool isTeleporting)
    {
        this.isTeleporting = isTeleporting;
    }

    public Transform GetModelTransform()
    {
        return model;
    }
}
