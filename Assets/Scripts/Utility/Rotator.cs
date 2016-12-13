using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    [SerializeField] private Vector3 rotationSpeeds;

    void FixedUpdate()
    {
        transform.Rotate(rotationSpeeds * Time.fixedDeltaTime);
    }

    public void ReverseDirection()
    {
        rotationSpeeds *= -1.0f;
    }
}
