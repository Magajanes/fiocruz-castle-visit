using System.Collections;
using UnityEngine;

public class InGameTutorial : Singleton<InGameTutorial>
{
    private bool _seenOpenDoorTutorial = false;
    
    [Header("References")]
    [SerializeField]
    private UIFader _uiFader;
    
    [Header("UI Elements")]
    [SerializeField]
    private GameObject _openDoorTutorial;

    public void ShowOpenDoorTutorial()
    {
        if (_seenOpenDoorTutorial)
            return;

        _seenOpenDoorTutorial = true;
        _uiFader.FadeIn(_openDoorTutorial, null, 2);
        StartCoroutine(OpenDoorTutorial());
    }

    private IEnumerator OpenDoorTutorial()
    {
        while (!Input.GetKeyDown(KeyCode.F))
            yield return null;

        _uiFader.FadeOut(_openDoorTutorial, null, 2);
    }
}
