using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    [SerializeField]
    private Vector3 rotationSpeeds;

    void FixedUpdate()
    {
        transform.Rotate(rotationSpeeds * Time.fixedDeltaTime);
    }
}
