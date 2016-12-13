using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;

	void Start () {
        direction = direction.normalized;
	}

	void FixedUpdate () {
        transform.Translate(direction * speed, Space.World);
	}
}
