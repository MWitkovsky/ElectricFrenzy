using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusAilmentDisplay : MonoBehaviour {

    private Image image;
    [SerializeField] private Sprite okSprite, poisonedSprite, paralyzedSprite, slowedSprite;

	void Start () {
        image = GetComponent<Image>();
        image.sprite = okSprite;
	}

    public void SetStatus(PlayerInfo.Status status)
    {
        if (status == PlayerInfo.Status.OK)
            image.sprite = okSprite;
        else if (status == PlayerInfo.Status.Paralyzed)
            image.sprite = paralyzedSprite;
        else if (status == PlayerInfo.Status.Poison)
            image.sprite = poisonedSprite;
        else if (status == PlayerInfo.Status.Slowed)
            image.sprite = slowedSprite;
    }
}
