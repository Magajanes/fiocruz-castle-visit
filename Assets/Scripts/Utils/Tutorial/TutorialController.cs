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
        Door.OnDoorOpen += () => CompleteTutorial(TutorialSubject.Door);
        Artifact.OnInteraction += (args) => CompleteTutorial(TutorialSubject.ArtifactInteraction);
    }

    private void Start()
    {
        ResourceRequest request = Resources.LoadAsync<TutorialConfig>(TUTORIAL_CONFIG_PATH);
        request.completed += (operation) => InitializeTutorialConfigDictionary(request.asset as TutorialConfig);
    }

    private void OnDestroy()
    {
        Door.OnDoorOpen -= () => CompleteTutorial(TutorialSubject.Door);
        Artifact.OnInteraction -= (args) => CompleteTutorial(TutorialSubject.ArtifactInteraction);
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
            UIFader.StopFadeOutCoroutine();
            UIFader.FadeIn(_tutorialPanel.gameObject);
            _tutorialPanel.SetMessage(message);
        }
    }

    public void HideTutorial(TutorialSubject subject)
    {
        if (!_tutorialSubjectMessages.ContainsKey(subject)) return;

        UIFader.StopFadeInCoroutine();
        UIFader.FadeOut(_tutorialPanel.gameObject, ClearMessage);
        
        void ClearMessage()
        {
            _tutorialPanel.SetMessage(string.Empty);
        }
    }

    public void CompleteTutorial(TutorialSubject subject)
    {
        if (!_tutorialSubjectMessages.ContainsKey(subject)) return;

        _tutorialSubjectMessages.Remove(subject);
        _tutorialPanel.Close();
    }
}
