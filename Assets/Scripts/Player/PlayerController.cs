using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = 15.0f, backwardsMoveSpeed = 13.5f, turnDelay = 1.0f;
    [SerializeField]
    private float attackTime = 0.15f, attackCooldown = 0.1f;
    [SerializeField]
    private float recoilForce, recoilTime, stunTime;

    private Rigidbody2D rb;
    private float attackTimer, attackCooldownTimer, turnTimer, recoilTimer, stunTimer;
    private bool isRecoiling, isStunned;
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
    }

    void Update()
    {
        //HANDLE DASH
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

        //TODO: replace true with actual condition for turning
        if (turnTimer > 0.0f && true)
            turnTimer -= Time.deltaTime;
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
            transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
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
