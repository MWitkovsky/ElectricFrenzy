using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollingTextContainer : MonoBehaviour {

    [SerializeField] private float scrollSpeed;
    [SerializeField] private bool right;

    private RectTransform rect;
    private float killTimer = 2.0f;
    private bool start, exit, firstRight;

	void Start () {
        rect = GetComponent<RectTransform>();

        if (right)
        {
            rect.position -= new Vector3(Screen.width, 0.0f, 0.0f);
            transform.GetChild(0).GetComponent<Text>().color = Color.clear;
            firstRight = true;
        }
        else
        {
            rect.position += new Vector3(Screen.width, 0.0f, 0.0f);
        }
        print(rect.position.x + " " + (rect.position.x < -6000.0f));
    }
	
	void Update () {
        if (start)
        {
            if (!exit)
            {
                if (firstRight)
                {
                    rect.position += Vector3.left * transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
                    transform.GetChild(0).GetComponent<Text>().color = Color.white;
                    firstRight = false;
                }
            }
            else
            {
                scrollSpeed += 1.0f;
                if (right)
                    rect.position += Vector3.right * scrollSpeed;
                else
                    rect.position += Vector3.left * scrollSpeed;

                killTimer -= Time.unscaledDeltaTime;
                if (killTimer <= 0.0f)
                    Destroy(gameObject);
            }
        }
    }

    public void Begin()
    {
        start = true;
        transform.GetChild(0).GetComponent<ScrollingText>().Begin();
    }

    public void Exit()
    {
        exit = true;
    }
}
