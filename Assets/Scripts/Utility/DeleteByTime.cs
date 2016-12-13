using UnityEngine;
using System.Collections;

public class DeleteByTime : MonoBehaviour {

    [SerializeField] private float time;

	void Update () {
        time -= Time.deltaTime;
        if (time <= 0.0f)
            Destroy(gameObject);
	}
}
