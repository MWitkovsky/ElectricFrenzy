using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealthBar : MonoBehaviour
{ 
    //Bar graphics
    [SerializeField] private GameObject fillGraphic;
    [SerializeField] private float drainSpeed;

    private Image healthBarImage;
    private float health;

    //For graphical fill
    private float displayHealth, targetDisplayHealth;
    private bool isTakingDamage;

    // Use this for initialization
    void Start()
    {
        if (fillGraphic == null)
            fillGraphic = GameObject.Find("BossHealthFill");

        healthBarImage = fillGraphic.GetComponent<Image>();

        displayHealth = 0.0f;
        targetDisplayHealth = 1.0f;
        health = 100.0f;
    }

    void FixedUpdate()
    {
        //Gives a more intense drain when taking damage versus a gentler fill when healing
        if (isTakingDamage)
            ProcessDamage();
        else if (displayHealth < targetDisplayHealth)
            ProcessHealthFill();
    }

    private void ProcessHealthFill()
    {
        displayHealth += drainSpeed * Time.deltaTime;
        if (displayHealth > targetDisplayHealth)
        {
            displayHealth = targetDisplayHealth;
        }

        UpdateGraphics();
    }

    private void ProcessDamage()
    {
        displayHealth -= drainSpeed * Time.deltaTime;
        if (displayHealth < targetDisplayHealth)
        {
            displayHealth = targetDisplayHealth;
            isTakingDamage = false;
        }

        UpdateGraphics();
    }


    private void UpdateGraphics()
    {
        healthBarImage.fillAmount = displayHealth;
    }

    //Takes two values and takes the resulting amount of damage
    //1.0f of rawDamage == 1% of total life
    public void ApplyDamage(float damage)
    {
        health -= damage;
        targetDisplayHealth = health / 100.0f;
        if (displayHealth < targetDisplayHealth)
            displayHealth = (health+damage) / 100.0f;

        if (health <= 0.0f)
        {
            health = 0.0f;
            targetDisplayHealth = 0.0f;

            //Die
        }

        isTakingDamage = true;
    }

    public float GetHealth()
    {
        return health;
    }
}
