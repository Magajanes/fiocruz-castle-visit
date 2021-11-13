using System;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialSubject
{
    Door,
    ArtifactInteraction,
    ArtifactCollection,
    ElevatorCall,
    ElevatorMove
}

public class TutorialController : Singleton<TutorialController>
{
    public const string TUTORIAL_CONFIG_PATH = "Tutorial/TutorialConfig";
    
    [SerializeField]
    private TutorialPanel _tutorialPanel;

    private Dictionary<TutorialSubject, string> _tutorialSubjectMessages = new Dictionary<TutorialSubject, string>();

    protected override void Awake()
    {
        base.Awake();
        AddListeners();
    }

    private void Start()
    {
        ResourceRequest request = Resources.LoadAsync<TutorialConfig>(TUTORIAL_CONFIG_PATH);
        request.completed += (operation) => InitializeTutorialConfigDictionary(request.asset as TutorialConfig);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void RemoveListeners()
    {
        Door.OnDoorOpen -= CompleteDoorTutorial;
        Artifact.OnInteraction -= CompleteArtifactInteractionTutorial;
        Elevator.OnElevatorCalled -= CompleteElevatorCallTutorial;
        Elevator.OnElevatorMoved -= CompleteElevatorMoveTutorial;
    }

    private void AddListeners()
    {
        RemoveListeners();
        Door.OnDoorOpen += CompleteDoorTutorial;
        Artifact.OnInteraction += CompleteArtifactInteractionTutorial;
        Elevator.OnElevatorCalled += CompleteElevatorCallTutorial;
        Elevator.OnElevatorMoved += CompleteElevatorMoveTutorial;
    }

    private void InitializeTutorialConfigDictionary(TutorialConfig config)
    {
        if (config == null) return;

        _tutorialSubjectMessages.Clear();
        foreach (TutorialConfigEntry entry in config.TutorialEntries)
        {
            _tutorialSubjectMessages[entry.Subject] = entry.Message;
        }
    }

    public void ShowTutorial(TutorialSubject subject)
    {
        if (_tutorialSubjectMessages.TryGetValue(subject, out string message))
        {
            _tutorialPanel.Open();
            _tutorialPanel.SetMessage(message);
        }
    }

    public void HideTutorial(TutorialSubject subject)
    {
        if (!_tutorialSubjectMessages.ContainsKey(subject)) return;

        _tutorialPanel.Close();
        _tutorialPanel.SetMessage(string.Empty);
    }

    private void CompleteDoorTutorial()
    {
        CompleteTutorial(TutorialSubject.Door);
    }

    private void CompleteArtifactInteractionTutorial(InitArgs args)
    {
        CompleteTutorial(TutorialSubject.ArtifactInteraction);
    }

    private void CompleteElevatorCallTutorial()
    {
        CompleteTutorial(TutorialSubject.ElevatorCall);
    }

    private void CompleteElevatorMoveTutorial()
    {
        CompleteTutorial(TutorialSubject.ElevatorMove);
    }


    public void CompleteTutorial(TutorialSubject subject)
    {
        if (!_tutorialSubjectMessages.ContainsKey(subject)) return;

        _tutorialPanel.Close();
        _tutorialSubjectMessages.Remove(subject);
    }
}
