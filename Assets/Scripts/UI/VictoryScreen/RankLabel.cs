using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankLabel : MonoBehaviour {

    [Tooltip("S - D")]
    public Sprite[] rankImages = new Sprite[5];

	public void SetImage(int index)
    {
        GetComponent<Image>().sprite = rankImages[index];
    }
}
