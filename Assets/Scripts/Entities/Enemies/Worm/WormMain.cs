using UnityEngine;
using System.Collections;

public class WormMain : MonoBehaviour {

    [SerializeField]
    private int packetYield, numOfSections;
    [SerializeField]
    private float moveSpeed, rotateSpeed;
    [SerializeField]
    private float snakeSpeed, snakeRotateSpeed;

    private GameObject head, body;

    public enum State { idle, spotted, attached };

	void Start () {
	    foreach (Transform t in transform)
        {
            if (t.gameObject.name == "Head")
                head = t.gameObject;
            else if (t.gameObject.name == "Body")
                body = t.gameObject;
        }

        for (int i=0; i<numOfSections; i++)
        {
            GameObject newSection = (GameObject)Instantiate(Resources.Load(ResourcePaths.SnakeBodySectionPrefab), 
                body.transform.position + new Vector3(1.0f * (i + 1), 0.0f, 0.0f), 
                body.transform.rotation);
            newSection.transform.parent = body.transform;
        }

        Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward);
        head.transform.forward = dir.normalized;
    }

	void FixedUpdate () {
        ////////////
        //MOVEMENT//
        ////////////
        head.transform.Translate(head.transform.forward * Time.fixedDeltaTime * moveSpeed, Space.World);

        Transform lastT = null;
        foreach (Transform t in body.transform)
        {
            if (!lastT)
            {
                t.position = Vector3.Lerp(t.position, head.transform.position-head.transform.forward, Time.fixedDeltaTime * snakeSpeed);
                t.rotation = Quaternion.Lerp(t.rotation, head.transform.rotation, Time.fixedDeltaTime * snakeRotateSpeed);
            }
            else
            {
                t.position = Vector3.Lerp(t.position, lastT.position - lastT.transform.forward, Time.fixedDeltaTime * snakeSpeed);
                t.rotation = Quaternion.Lerp(t.rotation, lastT.rotation, Time.fixedDeltaTime * snakeRotateSpeed);
            }

            lastT = t;
        }

        if (GameManager.IsDebugEnabled())
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                head.transform.Rotate(Time.fixedDeltaTime * rotateSpeed, 0.0f, 0.0f);
            else if (Input.GetKey(KeyCode.RightArrow))
                head.transform.Rotate(Time.fixedDeltaTime * -rotateSpeed, 0.0f, 0.0f);
        }
    }
}
