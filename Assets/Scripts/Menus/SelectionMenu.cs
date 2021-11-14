using UnityEngine;

public class SelectionMenu : MonoBehaviour
{
    public enum SelectionMenuState
    {
        NONE = 0,
        BOOK = 1,
        OPTIONS = 2,
        CREDITS = 3
    }

    private SelectionMenuState _currentState = SelectionMenuState.NONE;

    public void OnBookButtonClick()
    {
        if (_currentState == SelectionMenuState.BOOK) return;

        MenuController.Instance.ShowInventory(() => _currentState = SelectionMenuState.BOOK);
    }

    public void OnOptionsButtonClick()
    {
        if (_currentState == SelectionMenuState.OPTIONS) return;
        
        MenuController.Instance.ShowOptionsPanel(() => _currentState = SelectionMenuState.OPTIONS);
    }

    public void OnCreditsButtonClick()
    {
        if (_currentState == SelectionMenuState.CREDITS) return;

        MenuController.Instance.ShowCreditsPanel(() => _currentState = SelectionMenuState.CREDITS);
    }

    public void OnBackButtonClick()
    {
        if (_currentState == SelectionMenuState.NONE) return;

        MenuController.Instance.BackToMenu(() => _currentState = SelectionMenuState.NONE);
    }
}
