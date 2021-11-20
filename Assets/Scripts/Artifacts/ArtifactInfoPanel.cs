﻿using System.Collections.Generic;
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
    public const string ARTIFACT_INFO_PANEL_SOUNDS_BUNDLE_PATH = "SoundBundles/ArtifactPanelSounds";
    public const string ARTIFACT_IMAGE_PATH = "Sprites/Page/{0}";
    public const string PAGE_RIGHT_ID = "page_right";
    public const string PAGE_LEFT_ID = "page_left";
    public const string COLLECT_ARTIFACT_ID = "artifact_collected";

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
    [SerializeField]
    private GameObject leftArrow;
    [SerializeField]
    private GameObject rightArrow;

    private ArtifactInfo _currentInfo;
    private UIState _entryPoint;
    private Inventory _playerInventory;
    private List<int> _collectedArtifactsIds;

    private AudioClip _pageRightSound;
    private AudioClip _pageLeftSound;
    private AudioClip _collectArtifactSound;

    private void Start()
    {
        SoundsManager.LoadSoundsBundle(ARTIFACT_INFO_PANEL_SOUNDS_BUNDLE_PATH, OnSoundsLoaded);

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _pageRightSound = bundle.GetAudioClipById(PAGE_RIGHT_ID);
            _pageLeftSound = bundle.GetAudioClipById(PAGE_LEFT_ID);
            _collectArtifactSound = bundle.GetAudioClipById(COLLECT_ARTIFACT_ID);
        }
    }

    public void Initialize(InitArgs args)
    {
        _playerInventory = InventoryManager.PlayerInventory;
        _collectedArtifactsIds = _playerInventory.GetSortedCollectedArtifactsIds();
        _entryPoint = args.EntryPoint;

        int artifactId = args.ArtifactId;
        bool showCollectButton = _entryPoint == UIState.Inactive && !_playerInventory.HasArtifact(artifactId);
        bool showArrowsPanel = _playerInventory.HasArtifact(artifactId);

        collectButton.SetActive(showCollectButton);
        arrowsPanel.SetActive(showArrowsPanel);
        _currentInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null)
        {
            SetPanel();
            SetArrowsPanel(showArrowsPanel, _currentInfo.Id);
        }
    }

    public void Initialize(int artifactId)
    {
        _playerInventory = InventoryManager.PlayerInventory;
        _collectedArtifactsIds = _playerInventory.GetSortedCollectedArtifactsIds();

        bool showArrowsPanel = _playerInventory.HasArtifact(artifactId);

        collectButton.SetActive(false);
        _currentInfo = ArtifactsService.GetArtifactInfoById(artifactId);

        if (_currentInfo != null)
        {
            SetPanel();
            SetArrowsPanel(showArrowsPanel, _currentInfo.Id);
        }
    }

    private void SetPanel()
    {
        title.text = _currentInfo.Name;
        description.text = _currentInfo.Description;
        scrollRect.verticalNormalizedPosition = 1;

        ArtifactSpriteHelper.LoadArtifactSprite(
            string.Format(ARTIFACT_IMAGE_PATH,_currentInfo.ImageName),
            SetSprite
        );

        void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }

    private void SetArrowsPanel(bool showArrowsPanel, int artifactId)
    {
        arrowsPanel.SetActive(showArrowsPanel);

        if (showArrowsPanel)
        {
            leftArrow.SetActive(artifactId != _collectedArtifactsIds[0]);
            rightArrow.SetActive(artifactId != _collectedArtifactsIds[_collectedArtifactsIds.Count - 1]);
        }
    }

    public void ShowNextCollectedArtifact()
    {
        int index = _collectedArtifactsIds.IndexOf(_currentInfo.Id);
        rightArrow.SetActive(index < _collectedArtifactsIds.Count - 2);

        if (!leftArrow.activeInHierarchy) 
            leftArrow.SetActive(true);

        index++;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(_collectedArtifactsIds[index]);

        if (nextInfo != null)
        {
            _currentInfo = nextInfo;
            SetPanel();
        }

        SoundsManager.Instance.PlaySFX(_pageRightSound);
    }

    public void ShowPreviousCollectedArtifact()
    {
        int index = _collectedArtifactsIds.IndexOf(_currentInfo.Id);
        leftArrow.SetActive(index > 1);

        if (!rightArrow.activeInHierarchy)
            rightArrow.SetActive(true);

        index--;
        ArtifactInfo nextInfo = ArtifactsService.GetArtifactInfoById(_collectedArtifactsIds[index]);

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
        _collectedArtifactsIds = _playerInventory.GetSortedCollectedArtifactsIds();
        collectButton.SetActive(false);
        SetArrowsPanel(true, _currentInfo.Id);
        SoundsManager.Instance.PlaySFX(_collectArtifactSound);
    }
}
