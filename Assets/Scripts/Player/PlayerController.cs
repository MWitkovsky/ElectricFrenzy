using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashTime = 0.15f;
    [SerializeField]
    private float dashCooldown = 0.1f;

    private Rigidbody2D rb;
    private float attackTimer, dashTimer, dashCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dashTimer > 0.0f)
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = Vector2.zero;
            if (dashCooldown > 0.0f)
                dashCooldownTimer -= Time.deltaTime;
        }
            

        if(attackTimer > 0.0f)
            attackTimer -= Time.deltaTime;
    }

	public void Move(Vector2 move)
    {
        if(AbleToMove())
        {
            rb.velocity = Vector2.zero;
            transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Attack()
    {

    }

    public void Dash(Vector2 move)
    {
        if (AbleToMove())
        {
            dashTimer = dashTime;
            dashCooldownTimer = dashCooldown;
            rb.AddForce(move.normalized * moveSpeed * 2.0f, ForceMode2D.Impulse);
        }
    }

    private bool AbleToMove()
    {
        if (dashCooldownTimer <= 0.0f)
        {
            return true;
        }
        return false;
    }
}
