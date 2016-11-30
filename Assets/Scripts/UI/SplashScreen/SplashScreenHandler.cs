using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenHandler : MonoBehaviour {

    [SerializeField] private Color targetColor;
    [SerializeField] private float fadeInTime, stayTime, fadeOutTime, leftScrollSpawnTime, leftScrollStayTime, totalSplashTime;

    private Image image;
    private ScrollingTextContainer leftScrollingTextContainer, rightScrollingTextContainer;
    private float fadeInTimer, stayTimer, fadeOutTimer, leftScrollSpawnTimer, leftScrollStayTimer;

	void Start () { 
        Time.timeScale = 0.0f;
        totalSplashTime = fadeInTime + stayTime + fadeOutTime;

        image = GetComponent<Image>();
        leftScrollingTextContainer = GameObject.Find("ScrollingTextLeft").GetComponent<ScrollingTextContainer>();
        rightScrollingTextContainer = GameObject.Find("ScrollingTextRight").GetComponent<ScrollingTextContainer>();

        //init timers
        fadeInTimer = fadeInTime;
        stayTimer = stayTime;
        fadeOutTimer = fadeOutTime;

        leftScrollSpawnTimer = leftScrollSpawnTime;
        leftScrollStayTimer = leftScrollStayTime;
    }
	
	void Update () {
        if(leftScrollSpawnTimer > 0.0f)
        {
            leftScrollSpawnTimer -= Time.unscaledDeltaTime;
            if (leftScrollSpawnTimer <= 0.0f)
            {
                leftScrollingTextContainer.Begin();
                rightScrollingTextContainer.Begin();
            }  
        }
        else if(leftScrollStayTimer > 0.0f)
        {
            leftScrollStayTimer -= Time.unscaledDeltaTime;
            if (leftScrollStayTimer <= 0.0f)
            {
                leftScrollingTextContainer.Exit();
                rightScrollingTextContainer.Exit();
            }   
        }

        //FADEIN/OUT
	    if(fadeInTimer > 0.0f)
        {
            fadeInTimer -= Time.unscaledDeltaTime;

            if (fadeInTimer > 0.0f)
                image.color = Color.Lerp(Color.black, targetColor, (fadeInTime - fadeInTimer) / fadeInTime);
            else
                image.color = targetColor;
        }
        else if(stayTimer > 0.0f)
        {
            stayTimer -= Time.unscaledDeltaTime;
        }
        else if(fadeOutTimer > 0.0f)
        {
            fadeOutTimer -= Time.unscaledDeltaTime;

            if (fadeOutTimer > 0.0f)
                image.color = Color.Lerp(targetColor, Color.clear, (fadeOutTime - fadeOutTimer) / fadeOutTime);
            else
                image.color = Color.clear;
        }
        else
        {
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
	}
}
