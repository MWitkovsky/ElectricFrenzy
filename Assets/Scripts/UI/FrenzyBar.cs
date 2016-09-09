using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrenzyBar : MonoBehaviour
{ 
    [SerializeField]
    private GameObject fillGraphic;
    [SerializeField]
    private float fillSpeed;

    private Image frenzyBarImage;
    private float charge, displayCharge, targetDisplayCharge;
    private bool lerp;

    // Use this for initialization
    void Start()
    {
        if (fillGraphic == null)
            fillGraphic = GameObject.Find("FrenzyFill");

        frenzyBarImage = fillGraphic.GetComponent<Image>();
        charge = 0.0f;
    }

    void Update()
    {
        if (lerp)
        {
            displayCharge = Mathf.Lerp(displayCharge, targetDisplayCharge, fillSpeed * Time.deltaTime);
            if(Mathf.Abs(displayCharge - targetDisplayCharge) < 0.01f)
            {
                displayCharge = targetDisplayCharge;
                lerp = false;
            }
        }

        updateGraphics();

        //DEBUG CONTROLS
        if (GameManager.IsDebugEnabled())
            ProcessDebugControls();
    }

    private void AddFrenzyCharge(float charge)
    {
        this.charge += charge;
        if (this.charge > 100.0f)
            this.charge = 100.0f;

        targetDisplayCharge = this.charge / 100.0f;
        lerp = true;
    }

    private void RemoveFrenzyCharge(float charge)
    {
        this.charge -= charge;
        if (this.charge < 0.0f)
            this.charge = 0.0f;

        targetDisplayCharge = this.charge / 100.0f;
        lerp = true;
    }

    private void ProcessDebugControls()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            RemoveFrenzyCharge(20.0f);

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            AddFrenzyCharge(20.0f);
    }

    private void updateGraphics()
    {
        frenzyBarImage.fillAmount = displayCharge;
    }
}
