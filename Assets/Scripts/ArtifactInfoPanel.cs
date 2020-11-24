﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPanel : MonoBehaviour
{
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public abstract void Initialize(InitArgs args);

    public class InitArgs
    {
        public int ArtifactId;
        public UIState EntryPoint;

        public static InitArgs CreateWithId(int id)
        {
            return new InitArgs() { ArtifactId = id, EntryPoint = UIState.Inactive };
        }

        public static InitArgs Create(int id, UIState entryPoint)
        {
            return new InitArgs() { ArtifactId = id, EntryPoint = entryPoint };
        }
    }
}

public class ArtifactInfoPanel : UIPanel
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;
    [SerializeField]
    private GameObject collectButton;

    private ArtifactInfo _currentInfo;
    private UIState _entryPoint;

    public override void Initialize(InitArgs args)
    {
        InputController.OnBackButtonPress += ReturnToLastScreen;

        int artifactId = args.ArtifactId;
        _entryPoint = args.EntryPoint;
        collectButton.SetActive(_entryPoint == UIState.Inactive);

        if (ArtifactsService.TryGetArtifactInfo(artifactId, out ArtifactInfo artifactInfo))
        {
            _currentInfo = artifactInfo;
            SetPanel();
        }
    }

    private void SetPanel()
    {
        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        if (ArtifactsService.TryGetArtifactSprite(_currentInfo.Id, out Sprite sprite))
        {
            image.sprite = sprite;
        }
    }

    private void Close()
    {
        InputController.OnBackButtonPress -= ReturnToLastScreen;
        UIManager.ChangeState(UIState.Inactive);
    }

    private void ReturnToLastScreen()
    {
        InputController.OnBackButtonPress -= ReturnToLastScreen;
        UIManager.ChangeState(_entryPoint);
    }

    public void Collect()
    {
        InventoryService.SaveArtifact(_currentInfo.Id);
        Close();
    }
}
