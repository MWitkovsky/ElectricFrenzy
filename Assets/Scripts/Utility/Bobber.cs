using UnityEngine;
using System.Collections;

//Makes GameObject bob up and down in place
public class Bobber : MonoBehaviour {

    [SerializeField]
    private float bobDistance, bobSpeed;

    private float initYPosition;
    private float maxY, minY;
    private bool up;

	void Start () {
        initYPosition = transform.position.y;
        maxY = initYPosition + bobDistance;
        minY = initYPosition - bobDistance;
	}
	
	void FixedUpdate () {
        if (up)
        {
            transform.Translate(Vector2.up * bobSpeed * Time.fixedDeltaTime);
            if (transform.position.y > maxY)
                up = false;
        }
        else
        {
            transform.Translate(Vector2.down * bobSpeed * Time.fixedDeltaTime);
            if (transform.position.y < minY)
                up = true;
        }
	}
}
