using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitArgs
{
    public int ArtifactId;
    public UIState EntryPoint;

    public static InitArgs CreateWithId(int id)
    {
        return new InitArgs()
        {
            ArtifactId = id,
            EntryPoint = UIState.Inactive
        };
    }

    public static InitArgs Create(int id, UIState entryPoint)
    {
        return new InitArgs()
        {
            ArtifactId = id,
            EntryPoint = entryPoint
        };
    }
}

public class ArtifactInfoPanel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image image;
    [SerializeField]
    private ScrollRect scrollRect;

    [Header("Buttons Gameobjects")]
    [SerializeField]
    private GameObject collectButton;
    [SerializeField]
    private GameObject arrowsPanel;

    private ArtifactInfo _currentInfo;
    private UIState _entryPoint;
    private Inventory _playerInventory;
    private AudioClip _pageRightSound;
    private AudioClip _pageLeftSound;

    public void Initialize(InitArgs args)
    {
        _playerInventory = InventoryManager.PlayerInventory;
        _entryPoint = args.EntryPoint;

        int artifactId = args.ArtifactId;
        bool showCollectButton = _entryPoint == UIState.Inactive && !_playerInventory.HasArtifact(artifactId);
        bool showArrowsPanel = _playerInventory.HasArtifact(artifactId);

        collectButton.SetActive(showCollectButton);
        arrowsPanel.SetActive(showArrowsPanel);
        _currentInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null)
            SetPanel();
    }

    public void Initialize(int artifactId)
    {
        _playerInventory = InventoryManager.PlayerInventory;
        bool showArrowsPanel = _playerInventory.HasArtifact(artifactId);

        collectButton.SetActive(false);
        arrowsPanel.SetActive(showArrowsPanel);
        _currentInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null) SetPanel();
    }

    private void SetPanel()
    {
        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        scrollRect.verticalNormalizedPosition = 1;

        ArtifactSpriteHelper.LoadArtifactSprite(
            _currentInfo.ImagePath,
            SetSprite
        );

        SoundsManager.LoadSoundsBundle(
            ArtifactInfoPanelConstants.ARTIFACT_INFO_PANEL_SOUNDS_BUNDLE_PATH,
            OnSoundsLoaded
        );

        void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _pageRightSound = bundle.GetAudioClipById(ArtifactInfoPanelConstants.PAGE_RIGHT_ID);
            _pageLeftSound = bundle.GetAudioClipById(ArtifactInfoPanelConstants.PAGE_LEFT_ID);
        }
    }

    public void ShowNextCollectedArtifact()
    {
        List<int> collectedArtifactsIds = _playerInventory.GetSortedCollectedArtifactsIds();
        int index = collectedArtifactsIds.IndexOf(_currentInfo.Id);

        if (index == collectedArtifactsIds.Count - 1)
            return;

        index++;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(collectedArtifactsIds[index]);

        if (nextInfo != null)
        {
            _currentInfo = nextInfo;
            SetPanel();
        }

        SoundsManager.Instance.PlaySFX(_pageRightSound);
    }

    public void ShowPreviousCollectedArtifact()
    {
        List<int> collectedArtifactsIds = _playerInventory.GetSortedCollectedArtifactsIds();
        int index = collectedArtifactsIds.IndexOf(_currentInfo.Id);

        if (index <= 0)
            return;

        index--;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(collectedArtifactsIds[index]);

        if (nextInfo != null)
        {
            _currentInfo = nextInfo;
            SetPanel();
        }

        SoundsManager.Instance.PlaySFX(_pageLeftSound);
    }

    public void Collect()
    {
        _playerInventory.AddArtifact(_currentInfo.Id);
        collectButton.SetActive(false);
        arrowsPanel.SetActive(true);
    }
}

public static class ArtifactInfoPanelConstants
{
    public const string ARTIFACT_INFO_PANEL_SOUNDS_BUNDLE_PATH = "SoundBundles/ArtifactPanelSounds";
    public const string PAGE_RIGHT_ID = "page_right";
    public const string PAGE_LEFT_ID = "page_left";
}
