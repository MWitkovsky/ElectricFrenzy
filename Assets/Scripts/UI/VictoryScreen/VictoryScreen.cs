﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryScreen : MonoBehaviour {

    [Tooltip("index 0 is S rank threshold")]
    public float[] goalTimes = new float[5];
    [Tooltip("index 0 is S rank threshold")]
    public int[] goalPackets = new int[5];

    [SerializeField] private float messageDelay, timeDelay, packetsDelay, rankDelay, finalRankDelay;
    [SerializeField] private float messageTargetX, timeTargetX, packetsTargetX, finalRankTargetX;
    [SerializeField] private float fadeSpeed, flySpeed;

    private RectTransform messageTransform, timeTransform, packetsTransform, finalRankTransform;
    private Image overlay;
    private Color color;
    private float time, currentAlpha, targetAlpha;
    private float timeRankDelayTimer, packetRankDelayTimer, finalRankDelayTimer;
    private uint packets;
    private int timeScore, packetScore, totalScore;
    private bool messageDone, timeDone, packetsDone, finalRankDone;

	void Start () {
        messageTransform = GameObject.Find("VictoryMessage").GetComponent<RectTransform>();
        timeTransform = GameObject.Find("TimeLabel").GetComponent<RectTransform>();
        packetsTransform = GameObject.Find("PacketsLabel").GetComponent<RectTransform>();
        finalRankTransform = GameObject.Find("FinalRankLabel").GetComponent<RectTransform>();

        overlay = GetComponent<Image>();
        color = overlay.color;
        targetAlpha = overlay.color.a;
        timeRankDelayTimer = rankDelay;
        packetRankDelayTimer = rankDelay;
        finalRankDelayTimer = finalRankDelay;

        color.a = currentAlpha;
        overlay.color = color;
    }

	void Update () {
	    if(currentAlpha < targetAlpha)
        {
            currentAlpha += fadeSpeed * Time.deltaTime;
            if (currentAlpha > targetAlpha)
                currentAlpha = targetAlpha;

            color.a = currentAlpha;
            overlay.color = color;
        }

        if (messageDelay > 0.0f)
            messageDelay -= Time.deltaTime;
        else if (timeDelay > 0.0f)
            timeDelay -= Time.deltaTime;
        else if (packetsDelay > 0.0f)
            packetsDelay -= Time.deltaTime;
        else if (finalRankDelay > 0.0f)
            finalRankDelay -= Time.deltaTime;

        if(messageDelay <= 0.0f && !messageDone)
        {
            messageTransform.position = new Vector3(messageTransform.position.x + (flySpeed * Time.deltaTime), messageTransform.position.y, messageTransform.position.z);
            if(messageTransform.position.x > messageTargetX)
            {
                messageTransform.position = new Vector3(messageTargetX, messageTransform.position.y, messageTransform.position.z);
                messageDone = true;
            }
        }
        
        if(timeDelay > 0.0f && !timeDone)
        {
            timeTransform.position = new Vector3(timeTransform.position.x + (flySpeed * Time.deltaTime), timeTransform.position.y, timeTransform.position.z);
            if (timeTransform.position.x > timeTargetX)
            {
                timeTransform.position = new Vector3(timeTargetX, timeTransform.position.y, timeTransform.position.z);
                timeDone = true;
            }
        }
        else if (timeRankDelayTimer > 0.0f && timeDone)
        {
            timeRankDelayTimer -= Time.deltaTime;
            if(timeRankDelayTimer <= 0.0f)
            {
                //displayRank
            }
        }

        if (packetsDelay > 0.0f && !packetsDone)
        {
            packetsTransform.position = new Vector3(packetsTransform.position.x + (flySpeed * Time.deltaTime), packetsTransform.position.y, packetsTransform.position.z);
            if (packetsTransform.position.x > packetsTargetX)
            {
                packetsTransform.position = new Vector3(timeTargetX, packetsTransform.position.y, packetsTransform.position.z);
                packetsDone = true;
            }
        }
        else if (packetRankDelayTimer > 0.0f && packetsDone)
        {
            packetRankDelayTimer -= Time.deltaTime;
            if (packetRankDelayTimer <= 0.0f)
            {
                //displayRank
            }
        }

        if (finalRankDelay <= 0.0f && !finalRankDone)
        {
            finalRankTransform.position = new Vector3(finalRankTransform.position.x + (flySpeed * Time.deltaTime), finalRankTransform.position.y, finalRankTransform.position.z);
            if (finalRankTransform.position.x > finalRankTargetX)
            {
                finalRankTransform.position = new Vector3(finalRankTargetX, finalRankTransform.position.y, finalRankTransform.position.z);
                finalRankDone = true;
            }
        }
        else if (finalRankDelayTimer > 0.0f && finalRankDone)
        {
            finalRankDelayTimer -= Time.deltaTime;
            if(finalRankDelayTimer <= 0.0f)
            {
                //displayRank
            }
        }
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
