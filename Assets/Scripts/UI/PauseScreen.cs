using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseScreen : MonoBehaviour {

    //Don't really feel like writing this right now... bunch of buttons.
    //Mouse driven? Controller driven? Easy but tedious work.

	void Start () {
        gameObject.SetActive(false);
	}

    //IF ANY NON-REACTIVE TIME BASED EFFECTS OR LOGIC IS DONE, USE Time.unscaledDeltaTime
	void Update () {

	}
}
