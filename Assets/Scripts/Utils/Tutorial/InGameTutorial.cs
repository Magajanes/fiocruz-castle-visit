using System.Collections;
using UnityEngine;

public class InGameTutorial : Singleton<InGameTutorial>
{
    private bool _doorTutorialComplete = false;
    private bool _elevatorCallTutorialComplete = false;
    private bool _elevatorMoveTutorialComplete = false;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject _openDoorTutorial;
    [SerializeField]
    private GameObject _elevatorCallTutorial;
    [SerializeField]
    private GameObject _elevatorMoveTutorial;

    #region DoorTutorial
    private void ShowOpenDoorTutorial()
    {
        Door.OnDoorOpen += CompleteDoorTutorial;

        UIFader.FadeIn(_openDoorTutorial, null, 2);
        StartCoroutine(OpenDoorTutorial());
    }

    private void CompleteDoorTutorial()
    {
        Door.OnDoorOpen -= CompleteDoorTutorial;

        _doorTutorialComplete = true;
    }

    private IEnumerator OpenDoorTutorial()
    {
        while (!_doorTutorialComplete)
            yield return null;

        UIFader.FadeOut(_openDoorTutorial, null, 2);
    }
    #endregion

    #region ElevatorCallTutorial
    private void ShowElevatorCallTutorial()
    {
        UIFader.FadeIn(_elevatorCallTutorial, null, 2);
        StartCoroutine(ElevatorCallTutorial());
    }

    private void CompleteElevatorCallTutorial()
    {
        _elevatorCallTutorialComplete = true;
    }

    private IEnumerator ElevatorCallTutorial()
    {
        while (!_elevatorCallTutorialComplete)
            yield return null;

        UIFader.FadeOut(_elevatorCallTutorial, null, 2);
    }
    #endregion

    #region ElevatorMoveTutorial
    private void ShowElevatorMoveTutorial()
    {
        UIFader.FadeIn(_elevatorMoveTutorial, null, 2);
        StartCoroutine(ElevatorMoveTutorial());
    }

    private void CompleteElevatorMoveTutorial()
    {
        _elevatorMoveTutorialComplete = true;
    }

    private IEnumerator ElevatorMoveTutorial()
    {
        while (!_elevatorMoveTutorialComplete)
            yield return null;

        UIFader.FadeOut(_elevatorMoveTutorial, null, 2);
    }
    #endregion
}
