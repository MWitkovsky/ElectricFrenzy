using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	public void Move(Vector2 move)
    {
        transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
    }
}
