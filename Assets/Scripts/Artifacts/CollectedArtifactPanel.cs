using UnityEngine;
using UnityEngine.UI;

public class CollectedArtifactPanel : MonoBehaviour
{
    private const string STYLIZED_PICTURE_PATH = "Sprites/Stylized/{0}";
    private const string PICTURE_PATH = "Sprites/Final/{0}";
    private const float STYLE_TRANSITION_DELAY = 1;
    private const float STYLE_TRANSITION_TIME = 0.5f;
    
    [Header("Picture objects")]
    [SerializeField]
    private GameObject _flashObject;
    [SerializeField]
    private GameObject _stylizedPicture;

    [Header("Image references")]
    [SerializeField]
    private Image _stylizedImage;
    [SerializeField]
    private Image _pictureImage;

    [Header("Canvas group references")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CanvasGroup _flashCanvasGroup;
    [SerializeField]
    private CanvasGroup _stylizedCanvasGroup;

    private bool _stylizedSpriteLoaded = false;
    private bool _pictureSpriteLoaded = false;

    public void Initialize(ArtifactInfo artifactInfo)
    {
        _canvasGroup.alpha = 1;
        _flashCanvasGroup.alpha = 1;
        _stylizedCanvasGroup.alpha = 0;
        
        ArtifactSpriteHelper.LoadArtifactSprite(
            string.Format(STYLIZED_PICTURE_PATH, artifactInfo.ImageName),
            SetStylizedSprite
        );

        ArtifactSpriteHelper.LoadArtifactSprite(
            string.Format(PICTURE_PATH, artifactInfo.ImageName),
            SetPictureSprite
        );

        void SetStylizedSprite(Sprite sprite)
        {
            _stylizedSpriteLoaded = true;
            _stylizedImage.sprite = sprite;
            _stylizedImage.type = Image.Type.Simple;
            _stylizedImage.preserveAspect = true;

            ShowCollectedArtifactPanel();
        }

        void SetPictureSprite(Sprite sprite)
        {
            _pictureSpriteLoaded = true;
            _pictureImage.sprite = sprite;
            _pictureImage.type = Image.Type.Simple;
            _pictureImage.preserveAspect = true;

            ShowCollectedArtifactPanel();
        }

        void ShowCollectedArtifactPanel()
        {
            if (!_stylizedSpriteLoaded || !_pictureSpriteLoaded) return;

            UIFader.FadeOut(
                _flashObject, 
                () => Invoke("ShowStylizedPicture", STYLE_TRANSITION_DELAY),
                STYLE_TRANSITION_TIME
            );
        }
    }

    private void ShowStylizedPicture()
    {
        UIFader.FadeIn(
            _stylizedPicture,
            () => Invoke("CloseStylizedPicture", STYLE_TRANSITION_DELAY)
        );
    }

    private void CloseStylizedPicture()
    {
        UIFader.FadeOut(
            gameObject,
            Clean,
            STYLE_TRANSITION_TIME
        );
    }

    public void Clean()
    {
        _stylizedSpriteLoaded = false;
        _pictureSpriteLoaded = false;

        _stylizedImage.sprite = null;
        _pictureImage.sprite = null;

        gameObject.SetActive(false);
    }
}
