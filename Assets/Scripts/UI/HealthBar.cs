﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{ 
    [SerializeField]
    private GameObject fillGraphic, backGraphic;
    [SerializeField]
    private float damageDisplaySpeed, backBarHealSpeed, backBarEmptySpeed, backBarEmptyDelay;

    private Image healthBarImage, backBarImage;
    private float health;

    //for graphical LERP
    private Color damageColor, healColor;
    private int lerpCounter;
    private float initHealth, displayHealth, backBarHealth, targetDisplayHealth, backBarTargetHealth;
    private float backBarEmptyTimer;
    private bool isHealing, isTakingDamage, backHealBarDone;

    // Use this for initialization
    void Start()
    {
        if (fillGraphic == null)
            fillGraphic = GameObject.Find("HealthFill");
        if (backGraphic == null)
            backGraphic = GameObject.Find("HealthBack");

        healthBarImage = fillGraphic.GetComponent<Image>();
        backBarImage = backGraphic.GetComponent<Image>();
        damageColor = Color.cyan;
        healColor = Color.green;

        displayHealth = 1.0f;
        health = 100.0f;
        backBarHealth = 100.0f;
    }

    void Update()
    {
        if (GameManager.IsGameActive())
            ApplyTimeDamage(Time.deltaTime);

        //Gives a more intense drain when taking damage versus a gentler fill when healing
        if (isTakingDamage)
            ProcessDamage();
        else if (isHealing)
            ProcessHealing();

        if (backBarEmptyTimer > 0.0f)
            backBarEmptyTimer -= Time.deltaTime;

        if (backBarHealth > health && backBarEmptyTimer <= 0.0f)
            backBarHealth -= Time.deltaTime * backBarEmptySpeed;

        updateGraphics();

        //DEBUG CONTROLS
        if (GameManager.IsDebugEnabled())
            ProcessDebugControls();
    }

    private void ProcessDamage()
    {
        displayHealth = Mathf.Lerp(displayHealth, targetDisplayHealth, Time.deltaTime * damageDisplaySpeed);

        if (displayHealth - targetDisplayHealth < 0.01f)
        {
            displayHealth = targetDisplayHealth;
            isTakingDamage = false;
        }
    }

    private void ProcessHealing()
    {
        if (!backHealBarDone)
        {
            print(backBarHealth + " " + backBarTargetHealth);
            backBarHealth = Mathf.Lerp(backBarHealth, backBarTargetHealth, Time.deltaTime * backBarHealSpeed);

            if (Mathf.Abs(backBarHealth - backBarTargetHealth) < 0.01f)
            {
                backBarHealth = backBarTargetHealth;
                backHealBarDone = true;
            }
        }

        ++lerpCounter;
        displayHealth = Mathf.Lerp(initHealth, targetDisplayHealth, lerpCounter / 100.0f);

        if (lerpCounter == 100)
        {
            displayHealth = targetDisplayHealth;
            lerpCounter = 0;
            isHealing = false;
            backHealBarDone = false;
        }
    }

    private void ProcessDebugControls()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            ApplyDamage(20.0f);

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Heal(20.0f);
    }

    //Takes two values and takes the resulting amount of damage
    //1.0f of rawDamage == 1% of total life
    public void ApplyDamage(float damage)
    {
        backBarImage.color = damageColor;
        backBarEmptyTimer = backBarEmptyDelay;

        if (isHealing)
        {
            backBarHealth = displayHealth * 100.0f;
            isHealing = false;

            initHealth = displayHealth;
        }
        else
        {
            initHealth = health / 100.0f;
            displayHealth = initHealth;
        }

        health -= damage;
        targetDisplayHealth = health / 100.0f;

        if (health <= 0.0f)
        {
            health = 0.0f;
            targetDisplayHealth = 0.0f;

            //Die
        }

        isHealing = false;
        lerpCounter = 0;
        isTakingDamage = true;
    }

    public void ApplyTimeDamage(float damage)
    {
        if(health > 0.0f)
        {
            health -= damage;
            backBarHealth -= damage;
            backBarTargetHealth -= damage;
            targetDisplayHealth -= damage / 100.0f;
            displayHealth -= damage / 100.0f;
        }
    }

    public void Heal(float amount)
    {
        backHealBarDone = false;
        backBarImage.color = healColor;
        backBarHealth = health;

        if (health < 100.0f)
        {
            if (!isHealing)
            {
                initHealth = health / 100.0f; ;
                health += amount;
                backBarTargetHealth = health;
                targetDisplayHealth = health / 100.0f;

                displayHealth = initHealth;
            }
            else
            {
                //If heal effects gained in quick succession, compound the existing fill
                initHealth = displayHealth;
                health += amount;
                backBarTargetHealth = health;
                targetDisplayHealth = health / 100.0f;

                lerpCounter = 0;
            }
            //Clamp health to 100%
            if (health > 100.0f)
            {
                health = 100.0f;
                targetDisplayHealth = 1.00f;
            }

            lerpCounter = 0;
            isHealing = true;
        }        
    }

    private void updateGraphics()
    {
        healthBarImage.fillAmount = displayHealth;
        backBarImage.fillAmount = backBarHealth/100.0f;
    }
}
