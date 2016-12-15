using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButton : MonoBehaviour {

    [SerializeField] private Sprite buttonGraphic, highlightedButtonGraphic;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetHighlighted()
    {
        image.sprite = highlightedButtonGraphic;
    }

    public void SetUnhighlighted()
    {
        image.sprite = buttonGraphic;
    }
}
