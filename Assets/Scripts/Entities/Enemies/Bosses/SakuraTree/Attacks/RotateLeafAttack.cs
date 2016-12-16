using UnityEngine;
using System.Collections;

public class RotateLeafAttack : MonoBehaviour {

    [SerializeField] private int numOfLeaves;
    [SerializeField] private float spreadSpeed, rotateSpeed;

    private float despawnTimer = 25.0f;

    void Update()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0.0f)
            Destroy(gameObject);
    }

    private Vector3 DistritueCircle(Vector3 center, float radius, float index)
    {
        float ang = (index / numOfLeaves) * 360.0f;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public void Setup(int numOfLeaves, float spreadSpeed, float rotateSpeed)
    {
        this.numOfLeaves = numOfLeaves;
        this.spreadSpeed = spreadSpeed;
        this.rotateSpeed = rotateSpeed;

        LeafMain leaf;
        for (int i = 0; i < numOfLeaves; i++)
        {
            leaf = ((GameObject)Instantiate(Resources.Load(ResourcePaths.SakuraLeafPrefab), transform.position, transform.rotation)).GetComponent<LeafMain>();
            leaf.SetRotationHome(transform.position, DistritueCircle(transform.position, 1.0f, i), spreadSpeed, rotateSpeed);
            leaf.transform.parent = transform;
        }
    }
}
