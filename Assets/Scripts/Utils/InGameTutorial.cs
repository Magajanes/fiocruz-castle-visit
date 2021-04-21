using System.Collections;
using UnityEngine;

public class InGameTutorial : Singleton<InGameTutorial>
{
    private bool _doorTutorialComplete = false;
    private bool _elevatorCallTutorialComplete = false;
    private bool _elevatorMoveTutorialComplete = false;

    [Header("References")]
    [SerializeField]
    private UIFader _uiFader;
    
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _openDoorTutorial;
    [SerializeField]
    private GameObject _elevatorCallTutorial;
    [SerializeField]
    private GameObject _elevatorMoveTutorial;

    private void Start()
    {
        Door.OnDoorReached += ShowOpenDoorTutorial;
        ElevatorDoor.OnElevatorReached += ShowElevatorCallTutorial;
        ElevatorTrigger.OnElevatorEnter += ShowElevatorMoveTutorial;
    }

    #region DoorTutorial
    private void ShowOpenDoorTutorial()
    {
        Door.OnDoorReached -= ShowOpenDoorTutorial;
        Door.OnDoorOpen += CompleteDoorTutorial;

        _uiFader.FadeIn(_openDoorTutorial, null, 2);
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

        _uiFader.FadeOut(_openDoorTutorial, null, 2);
    }
    #endregion

    #region ElevatorCallTutorial
    private void ShowElevatorCallTutorial()
    {
        ElevatorDoor.OnElevatorReached -= ShowElevatorCallTutorial;
        Elevator.OnElevatorCalled += CompleteElevatorCallTutorial;

        _uiFader.FadeIn(_elevatorCallTutorial, null, 2);
        StartCoroutine(ElevatorCallTutorial());
    }

    private void CompleteElevatorCallTutorial()
    {
        Elevator.OnElevatorCalled -= CompleteElevatorCallTutorial;

        _elevatorCallTutorialComplete = true;
    }

    private IEnumerator ElevatorCallTutorial()
    {
        while (!_elevatorCallTutorialComplete)
            yield return null;

        _uiFader.FadeOut(_elevatorCallTutorial, null, 2);
    }
    #endregion

    #region ElevatorMoveTutorial
    private void ShowElevatorMoveTutorial()
    {
        ElevatorTrigger.OnElevatorEnter -= ShowElevatorMoveTutorial;
        Elevator.OnElevatorMoved += CompleteElevatorMoveTutorial;

        _uiFader.FadeIn(_elevatorMoveTutorial, null, 2);
        StartCoroutine(ElevatorMoveTutorial());
    }

    private void CompleteElevatorMoveTutorial()
    {
        Elevator.OnElevatorMoved -= CompleteElevatorMoveTutorial;

        _elevatorMoveTutorialComplete = true;
    }

    private IEnumerator ElevatorMoveTutorial()
    {
        while (!_elevatorMoveTutorialComplete)
            yield return null;

        _uiFader.FadeOut(_elevatorMoveTutorial, null, 2);
    }
    #endregion
}
