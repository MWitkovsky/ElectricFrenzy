using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownIcon : MonoBehaviour {

    private Image fillIcon;

    void Start()
    {
        fillIcon = transform.GetChild(0).GetComponent<Image>();
    }

	public void UpdateDisplay(float fillAmount)
    {
        fillIcon.fillAmount = fillAmount;
    }
}
