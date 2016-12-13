using UnityEngine;
using System.Collections;

public class AdwareHomeRotation : MonoBehaviour {
    
    [SerializeField] private float rotateSpeed;

    private Transform home;
    private Vector3 radius;

    void Start()
    {
        home = transform.parent;
        radius = transform.position - home.position;
    }

    void FixedUpdate()
    {
        radius = Quaternion.AngleAxis(rotateSpeed * Time.fixedDeltaTime, Vector3.forward) * radius;
        transform.position = home.position + radius;
    }
}
