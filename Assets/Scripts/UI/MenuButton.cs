using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour {

    [SerializeField] private Sprite buttonGraphic, highlightedButtonGraphic;

    public void SetHighlighted()
    {
        GetComponent<Image>().sprite = highlightedButtonGraphic;
    }

    public void SetUnhighlighted()
    {
        GetComponent<Image>().sprite = buttonGraphic;
    }
}
