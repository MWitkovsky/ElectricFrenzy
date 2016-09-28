using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Basic Movement
    [SerializeField]
    private float moveSpeed = 15.0f, backwardsMoveSpeed = 13.5f, turnDelay = 1.0f;

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
    private float attackTimer, attackCooldownTimer, teleportCooldownTimer, turnTimer, recoilTimer, stunTimer;
    private bool isRecoiling, isStunned;
    private bool facingRight;

    //animation variables
    private Animator anim;
    private Vector2 currentMoveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;
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
            if (attackCooldown > 0.0f)
                attackCooldownTimer -= Time.deltaTime;
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

        //TODO: replace true with actual condition for turning
        if (turnTimer > 0.0f && true)
            turnTimer -= Time.deltaTime;
        else
            turnTimer = turnDelay;

        //Anim flags
        anim.SetFloat("horizontalMoveSpeed", currentMoveSpeed.x);
        anim.SetFloat("verticalMoveSpeed", currentMoveSpeed.y);
    }

    void FixedUpdate()
    {
        if (ShouldLockVelocity())
        {
            rb.velocity = Vector2.zero;
        }
    }

	public void Move(Vector2 move)
    {
        if (AbleToMove())
        {
            currentMoveSpeed = move * moveSpeed * Time.fixedDeltaTime;
            transform.Translate(currentMoveSpeed);
        }
    }

    public void Attack(Vector2 move)
    {
        if (AbleToMove())
        {
            attackTimer = attackTime;
            attackCooldownTimer = attackCooldown;
            rb.AddForce(move.normalized * moveSpeed * 2.0f, ForceMode2D.Impulse);
        }
    }

    public void Teleport(Vector2 move)
    {
        if (AbleToMove() && teleportCooldownTimer <= 0.0f)
        {
            PlayerManager.DetachKeyloggers();
            transform.Translate(move.normalized * teleportDistance, Space.World);
            teleportCooldownTimer = teleportCooldown;
        }
    }

    public void TakeHit()
    {
        if (CanTakeHit())
        {
            if (facingRight)
                rb.AddForce(Vector2.left * recoilForce, ForceMode2D.Impulse);
            else
                rb.AddForce(Vector2.right * recoilForce, ForceMode2D.Impulse);

            recoilTimer = recoilTime;
            isRecoiling = true;
        }
    }

    public bool IsFacingRight()
    {
        return facingRight;
    }

    public bool IsAttacking()
    {
        return attackTimer > 0.0f;
    } 

    private bool AbleToMove()
    {
        if (attackCooldownTimer <= 0.0f && !isRecoiling && !isStunned)
        {
            return true;
        }
        return false;
    }

    private bool ShouldLockVelocity()
    {
        if (attackTimer <= 0.0f && recoilTimer <= 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool CanTakeHit()
    {
        if (isRecoiling)
        {
            return false;
        }
        return true;
    }
}
