using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingText : MonoBehaviour {

    [SerializeField] private string displayText;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private bool right;

    private RectTransform rect;
    private Text text;
    private bool start;

	void Start () {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();

        for(int i=0; i<25; i++)
        {
            text.text += displayText;
        }
	}
	
	void Update () {
        if (start)
        {
            if (right)
                rect.position += Vector3.right * scrollSpeed;
            else
                rect.position += Vector3.left * scrollSpeed;
        }
	}

    public void Begin()
    {
        start = true;
    }
}
