using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenHandler : MonoBehaviour {

    [SerializeField] private Color targetColor;
    [SerializeField] private float fadeInTime, stayTime, fadeOutTime, totalSplashTime;

    private Image image;
    private float fadeInTimer, stayTimer, fadeOutTimer;

	void Start () { 
        Time.timeScale = 0.0f;
        totalSplashTime = fadeInTime + stayTime + fadeOutTime;

        image = GetComponent<Image>();

        //init timers
        fadeInTimer = fadeInTime;
        stayTimer = stayTime;
        fadeOutTimer = fadeOutTime;
	}
	
	void Update () {
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
