using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryScreen : MonoBehaviour {

    [Tooltip("index 0 is S rank threshold")]
    public float[] goalTimes = new float[5];
    [Tooltip("index 0 is S rank threshold")]
    public int[] goalPackets = new int[5];

    [SerializeField] private float fadeSpeed;

    private Image overlay;
    private float time, currentAlpha, targetAlpha;
    private uint packets;
    private int timeScore, packetScore, totalScore;

	void Start () {
        overlay = GetComponent<Image>();
        targetAlpha = overlay.color.a;
	}

	void Update () {
	    
	}

    public void Begin()
    {
        GameManager.SetGameActive(false);
        time = UIManager.GetTime();
        packets = PlayerManager.GetNumOfLoosePackets();
        CalculateScore();
        gameObject.SetActive(true);
    }

    private void CalculateScore()
    {
        for(int i=0; i<goalTimes.Length; i++)
        {
            if (time < goalTimes[i])
                timeScore += goalTimes.Length - i;
        }
        for(int i=0; i<goalPackets.Length; i++)
        {
            if (packets > goalPackets[i])
                packetScore += goalPackets.Length - i;
        }

        totalScore = (int)Mathf.Floor((timeScore + packetScore)/2.0f);
    }
}
