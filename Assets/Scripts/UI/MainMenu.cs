using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MenuButton[] buttons;

    private int selectedOption;
    private bool stickReset;

    // Use this for initialization
    void Start()
    {
        selectedOption = 0;
    }

    void Update()
    {
        for(int i=0; i<buttons.Length; i++)
        {
            if (i == selectedOption)
                buttons[i].SetHighlighted();
            else
                buttons[i].SetUnhighlighted();
        }

        float v = Input.GetAxisRaw("Vertical");
        if (stickReset)
        {
            if (v > 0.5f)
            {
                GameManager.GetPauseScreen().MoveUpMenu();
                stickReset = false;
            }
            else if (v < -0.5f)
            {
                GameManager.GetPauseScreen().MoveDownMenu();
                stickReset = false;
            }
        }

        if (Mathf.Abs(v) < 0.1f)
            stickReset = true;

        if (Input.GetButtonDown("Fire1"))
            SelectMenuOption();
    }

    public void SelectMenuOption()
    {
        switch (selectedOption)
        {
            case 0:
                GameManager.ChangeScene(1);
                break;
            case 1:
                GameManager.ChangeScene(1);
                break;
            case 2:
                GameManager.ExitGame();
                break;
        }
    }

    public void MoveUpMenu()
    {
        buttons[selectedOption].SetUnhighlighted();
        if (--selectedOption == -1)
            selectedOption = buttons.Length - 1;
        buttons[selectedOption].SetHighlighted();
    }

    public void MoveDownMenu()
    {
        buttons[selectedOption].SetUnhighlighted();
        if (++selectedOption == buttons.Length)
            selectedOption = 0;
        buttons[selectedOption].SetHighlighted();
    }
}
