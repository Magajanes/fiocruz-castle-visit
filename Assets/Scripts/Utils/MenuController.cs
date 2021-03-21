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
    private GameObject mainMenu;
    [SerializeField]
    private GameObject mainText;

    public static event Action OnGameStart; 

    private void Awake()
    {
        //Sound starts here
    }

    private void Start()
    {
        uiFader.FadeIn(
            mainMenu,
            OnFadeFinish,
            0.25f
        );

        void OnFadeFinish()
        {
            uiFader.FadeIn(
                mainText,
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
