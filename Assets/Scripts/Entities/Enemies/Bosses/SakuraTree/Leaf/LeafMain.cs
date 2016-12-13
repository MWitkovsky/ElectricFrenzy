using UnityEngine;
using System.Collections;

public class LeafMain : MonoBehaviour {

    private Rotator rotator;
    private Mover mover;
    private Vector3 home;

    //if doing a rotation attack
    private Vector3 radius;
    private float spreadSpeed, rotateSpeed;
    private bool straightPath = true;

	void Awake () {
        rotator = GetComponent<Rotator>();
        mover = GetComponent<Mover>();
	}

    void FixedUpdate()
    {
        if (!straightPath)
        {
            radius = (Quaternion.AngleAxis(rotateSpeed * Time.fixedDeltaTime, Vector3.forward) * radius) + (radius.normalized * spreadSpeed * Time.fixedDeltaTime);
            transform.position = home + radius;
        }
    }

    public void SetRotationHome(Vector3 home, Vector3 initSpread, float spreadSpeed, float rotateSpeed)
    {
        this.home = home;
        transform.position = initSpread;
        radius = initSpread - home;
        this.spreadSpeed = spreadSpeed;
        this.rotateSpeed = rotateSpeed;
        straightPath = false;
        mover.enabled = false;
    }

    public void ReverseRotation()
    {
        rotator.ReverseDirection();
    }
}
