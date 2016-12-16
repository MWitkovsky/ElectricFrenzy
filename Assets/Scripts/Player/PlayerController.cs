using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Basic Movement
    [SerializeField]
    private float moveSpeed = 15.0f, backwardsMoveSpeed = 13.5f, turnDelay, acceleration = 1.0f, turnSpeed = 0.5f;

    //Status Ailments
    [SerializeField]
    private float slowDownStatusPercentage = 50.0f;

    //Attack
    [SerializeField]
    private float attackTime = 0.15f, attackCooldown = 0.1f;

    //Teleport
    [SerializeField]
    private float teleportDistance, teleportCooldown, unusedTeleportTravelTime, unusedTeleportStun;

    //Timers
    [SerializeField]
    private float recoilForce, recoilTime, stunTime;

    //Sound Effects
    [SerializeField]
    private AudioClip teleportSound, hackSound, takeHitSound, shieldSound, proxySound, frenzySound, rechargeSound, packetCollectSound, packetAttractSound, killedEnemy, deathSound;

    //Not set in editor
    private Rigidbody2D rb;
    private AudioSource source;
    private Vector2 lastAttack;
    private float attackTimer, attackCooldownTimer, teleportCooldownTimer, turnTimer, recoilTimer, stunTimer;
    private bool isRecoiling, isStunned, isTeleporting;

    //animation variables
    private Animator anim;
    private Transform model, hitbox;
    private Vector2 currentMoveSpeed;
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        foreach (Transform t in transform)
        {
            if (t.name == "Mesh")
                model = t;
            else if (t.name == "Hitbox")
                hitbox = t;
        }
        facingRight = true;

        slowDownStatusPercentage /= 100.0f;

        GetComponent<AfterimageGenerator>().SetModelTransform(model);
    }

    void Update()
    {
        //HANDLE ATTACK
        if (attackTimer > 0.0f)
        {
            Instantiate(Resources.Load(ResourcePaths.HackFXPrefab), transform.position, transform.localRotation);
            attackTimer -= Time.deltaTime;
        }
        else
        {
            if(attackCooldownTimer > 0.0f)
            {
                attackCooldownTimer -= Time.deltaTime;
                if(attackCooldownTimer <= 0.0f)
                    UIManager.UpdateHackCooldownIconDisplay(1.0f);
                else
                    UIManager.UpdateHackCooldownIconDisplay(1.0f - attackCooldownTimer / attackCooldown);
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
        {
            teleportCooldownTimer -= Time.deltaTime;

            if(teleportCooldownTimer <= 0.0f)
                UIManager.UpdateTeleportCooldownIconDisplay(1.0f);
            else
                UIManager.UpdateTeleportCooldownIconDisplay(1.0f - teleportCooldownTimer/teleportCooldown);
        }
            

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
            rb.velocity = Vector2.zero;
        }
    }

	public void Move(Vector2 move)
    {
        if (AbleToMove())
        {
            float tiltPercent = move.magnitude;
            if (PlayerManager.GetStatus() == PlayerInfo.Status.Slowed)
                tiltPercent *= slowDownStatusPercentage;

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
        if (AbleToMove() && attackCooldownTimer <= 0.0f && move != Vector2.zero)
        {
            attackTimer = attackTime;
            rb.velocity = Vector3.zero;
            rb.AddForce(move.normalized * moveSpeed * 2.0f, ForceMode2D.Impulse);

            lastAttack = move.normalized;
            model.LookAt(transform.position + (Vector3)move.normalized);

            //rotate hitbox
            Vector3 difference = (transform.position + (Vector3)move.normalized) - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            hitbox.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            hitbox.Rotate(0.0f, 0.0f, -90.0f);

            if (!PlayerManager.IsFrenzying())
            {
                attackCooldownTimer = attackCooldown;
                UIManager.UpdateHackCooldownIconDisplay(0.0f);
            }            

            anim.SetTrigger("attack");
            source.PlayOneShot(hackSound);
        }
    }

    public void FinishAttack()
    {
        if (Vector2.Dot(lastAttack, Vector2.left) > 0)
            SetFacingRight(false);
        else
            SetFacingRight(true);
    }

    public void StartRecoil()
    {
        if (Vector2.Dot(transform.forward, Vector2.left) > 0)
            SetFacingRight(false);
        else
            SetFacingRight(true);
    }

    public void ResetAttackCooldown()
    {
        attackCooldownTimer = 0.0f;
        UIManager.UpdateHackCooldownIconDisplay(1.0f);
    }

    public void Teleport(Vector2 move)
    {
        if (AbleToMove() && teleportCooldownTimer <= 0.0f && move != Vector2.zero)
        {
            PlayerManager.DetachEnemies();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, move.normalized, teleportDistance, LayerMask.GetMask("Walls", "Enemies"));

            Instantiate(Resources.Load(ResourcePaths.TeleportFXPrefab), transform.position, Quaternion.identity);

            if (hit.collider != null)
                transform.position = hit.point - move.normalized/2.0f;
            else
                transform.Translate(move.normalized * teleportDistance, Space.World);

            Instantiate(Resources.Load(ResourcePaths.TeleportFXPrefab), transform.position, Quaternion.identity);

            teleportCooldownTimer = teleportCooldown;
            UIManager.UpdateTeleportCooldownIconDisplay(0.0f);
            source.PlayOneShot(teleportSound);
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
            source.PlayOneShot(takeHitSound);
        }
    }

    private bool AbleToMove()
    {
        if (attackTimer <= 0.0f && !isRecoiling && !isStunned && !isTeleporting && PlayerManager.GetStatus() != PlayerInfo.Status.Paralyzed)
            return true;
        else
            return false;
    }

    private bool ShouldLockVelocity()
    {
        if (PlayerManager.GetStatus() == PlayerInfo.Status.Paralyzed)
            return true;
        else
            return false;
    }

    public bool CanTakeHit()
    {
        if (isRecoiling || isTeleporting)
            return false;
        else
            return true;
    }

    public void SetParticles(bool toggle)
    {
        ParticleSystem bwps = GetComponentInChildren<ParticleSystem>();

        if (toggle)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            bwps.Play();
        }
        else
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);  
            bwps.Stop();
        }
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

        hitbox.eulerAngles = Vector3.zero;
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
        rb.velocity = Vector3.zero;
    }

    public Transform GetModelTransform()
    {
        return model;
    }

    //play various sounds
    public void PlayPacketCollectSound()
    {
        source.PlayOneShot(packetCollectSound);
    }

    public void PlayPacketAttractSound()
    {
        source.PlayOneShot(packetAttractSound);
    }

    public void PlayShieldSound()
    {
        source.PlayOneShot(shieldSound);
    }

    public void PlayProxySound()
    {
        source.PlayOneShot(proxySound);
    }

    public void PlayFrenzySound()
    {
        source.PlayOneShot(frenzySound);
    }
    
    public void PlayRechargeSound()
    {
        source.PlayOneShot(rechargeSound);
    }

    public void PlayKillEnemySound()
    {
        source.PlayOneShot(killedEnemy);
    }

    public AudioClip GetDeathSound()
    {
        return deathSound;
    }
}
