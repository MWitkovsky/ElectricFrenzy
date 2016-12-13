using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;

	void Start () {
        direction = direction.normalized;
	}

	void FixedUpdate () {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
	}

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
