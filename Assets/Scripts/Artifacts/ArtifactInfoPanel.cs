using System.Collections.Generic;
using TMPro;
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
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;

    [Header("Buttons Gameobjects")]
    [SerializeField]
    private GameObject collectButton;
    [SerializeField]
    private GameObject arrowsPanel;

    private ArtifactInfo _currentInfo;
    private UIState _entryPoint;

    public override void Initialize(InitArgs args)
    {
        InputController.OnBackButtonPress += ReturnToLastScreen;

        int artifactId = args.ArtifactId;
        _entryPoint = args.EntryPoint;
        bool showCollectButton = _entryPoint == UIState.Inactive && !InventoryService.IsArtifactCollected(artifactId);
        bool showArrowsPanel = InventoryService.IsArtifactCollected(artifactId);
        collectButton.SetActive(showCollectButton);
        arrowsPanel.SetActive(showArrowsPanel);
        _currentInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null)
            SetPanel();
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

    public void ShowNextCollectedArtifact()
    {
        int index = InventoryService.GetArtifactIndex(_currentInfo.Id);
        List<int> collectedArtifactsIds = InventoryService.GetCollectedArtifactsIds();

        if (index == collectedArtifactsIds.Count - 1)
            return;

        index++;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(collectedArtifactsIds[index]);

        if (nextInfo != null)
        {
            _currentInfo = nextInfo;
            SetPanel();
        }
    }

    public void ShowPreviousCollectedArtifact()
    {
        int index = InventoryService.GetArtifactIndex(_currentInfo.Id);
        List<int> collectedArtifactsIds = InventoryService.GetCollectedArtifactsIds();

        if (index <= 0)
            return;

        index--;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(collectedArtifactsIds[index]);

        if (nextInfo != null)
        {
            _currentInfo = nextInfo;
            SetPanel();
        }
    }

    public void Collect()
    {
        InventoryService.SaveArtifact(_currentInfo.Id);
        collectButton.SetActive(false);
        arrowsPanel.SetActive(true);
    }
}
