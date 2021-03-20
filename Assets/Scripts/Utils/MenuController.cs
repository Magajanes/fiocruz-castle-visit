using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private bool _inputLock = true;
    
    [Header("References")]
    [SerializeField]
    private UIFader uiFader;

    [Header("UI Elements")]
    [SerializeField]
    private CanvasGroup mainMenuCanvasGroup;
    [SerializeField]
    private CanvasGroup mainTextCanvasGroup;

    public static event Action OnGameStart; 

    private void Awake()
    {
        //Sound starts here
    }

    private void Start()
    {
        uiFader.FadeIn(
            mainMenuCanvasGroup,
            OnFadeFinish,
            0.25f
        );

        void OnFadeFinish()
        {
            uiFader.FadeIn(
                mainTextCanvasGroup,
                () => _inputLock = false
            );
        }
    }

    private void Update()
    {
        if (_inputLock)
            return;
        
        if (Input.anyKeyDown)
        {
            OnGameStart?.Invoke();
        }
    }
}
