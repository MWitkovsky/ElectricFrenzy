using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

    //Timer sprites
    [SerializeField]
    private Image[] timerSlots = new Image[6];
    [SerializeField]
    private Sprite[] numbers = new Sprite[10];

    //Converts time to 00:00.00 format
    public void UpdateTimeDisplay(float time)
    {
        //This check is just for sanity, otherwise a -1 will appear after the display after 0.0
        //if (time <= 0.0f)
        //{
        //timeDisplay.text = "00:00.00";
        //}

        //strings for handling leading zeroes if needed
        string minutes = "";
        string seconds = "";
        string milliseconds = "";

        int minutesI = (int)(time / 60.0f);
        time %= 60;
        int secondsI = (int)time;
        time %= 1.0f;
        int millisecondsI = (int)(time * 100);

        //adding on leading zeroes if needed
        if (minutesI < 10)
            minutes = "0" + minutesI.ToString();
        else
            minutes = minutesI.ToString();

        if (secondsI < 10)
            seconds = "0" + secondsI.ToString();
        else
            seconds = secondsI.ToString();

        if (millisecondsI < 10)
            milliseconds = "0" + millisecondsI.ToString();
        else
            milliseconds = millisecondsI.ToString();

        //timeDisplay.text = (minutes + ":" + seconds + "." + milliseconds);

        setTimerImage(timerSlots[0], minutes.Substring(0, 1));
        setTimerImage(timerSlots[1], minutes.Substring(1, 1));
        setTimerImage(timerSlots[2], seconds.Substring(0, 1));
        setTimerImage(timerSlots[3], seconds.Substring(1, 1));
        setTimerImage(timerSlots[4], milliseconds.Substring(0, 1));
        setTimerImage(timerSlots[5], milliseconds.Substring(1, 1));
    }

    private void setTimerImage(Image slot, string s)
    {
        int num;
        int.TryParse(s, out num);
        slot.sprite = numbers[num];
    }
}
