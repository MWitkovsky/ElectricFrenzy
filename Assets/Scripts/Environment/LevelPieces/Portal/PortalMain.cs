using UnityEngine;
using System.Collections;

public class PortalMain : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
        transform.Rotate(new Vector3(0.0f, 9.5f * Time.deltaTime, 0.0f));
	}

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag.Equals("Player"))
        {
            //GameManager.NextLevel();
            GameManager
        }
    }
}
