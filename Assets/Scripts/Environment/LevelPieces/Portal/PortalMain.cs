using UnityEngine;
using System.Collections;

public class PortalMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0.0f, 9.5f * Time.deltaTime, 0.0f));
	}

    void OnTriggerEnter2D(Collider2D obj)
    {
        Debug.Log("Entered");
        if(obj.gameObject.tag.Equals("Player"))
        {
            GameManager.NextLevel();
        }
    }
}
