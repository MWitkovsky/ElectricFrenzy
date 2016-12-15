using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScreen : MonoBehaviour {

    [Tooltip("index 0 is unpause, 1 is restart level, 2 is return to main menu")]
    [SerializeField] private MenuButton[] buttons;

    private int selectedOption;
    private bool start = true;

	void Start () {
        selectedOption = 0;
        start = false;
	}

    public void SelectMenuOption()
    {
        switch (selectedOption)
        {
            case 0:
                GameManager.TogglePauseGame();
                break;
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 2:
                //return to main menu
                break;
        }
    }

    public void MoveUpMenu()
    {
        buttons[selectedOption].SetUnhighlighted();
        if (--selectedOption == -1)
            selectedOption = buttons.Length-1;
        buttons[selectedOption].SetHighlighted();
    }

    public void MoveDownMenu()
    {
        buttons[selectedOption].SetUnhighlighted();
        if (++selectedOption == buttons.Length)
            selectedOption = 0;
        buttons[selectedOption].SetHighlighted();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        if (active)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == 0)
                    buttons[i].SetHighlighted();
                else
                    buttons[i].SetUnhighlighted();
            }
        }
    }
}
