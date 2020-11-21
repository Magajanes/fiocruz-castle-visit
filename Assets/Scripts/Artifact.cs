using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private int _atifactId;
    private string _name;
    private string _description;
    private Sprite _sprite;

    public string Name
    {
        get
        {
            if (_name == null)
            {
                if (ArtifactsService.TryGetArtifactName(_atifactId, out _name))
                {
                    return _name;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _name;
        }
    }

    public string Description
    {
        get
        {
            if (_description == null)
            {
                if (ArtifactsService.TryGetArtifactDescription(_atifactId, out _description))
                {
                    return _description;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _description;
        }
    }

    public Sprite Sprite
    {
        get
        {
            if (_sprite == null)
            {
                if (ArtifactsService.TryGetArtifactSprite(_atifactId, out _sprite))
                {
                    return _sprite;
                }
                else
                {
                    return null;
                }
            }
            return _sprite;
        }
    }

    private void ShowInfo()
    {
        UIManager.ChangeState(
            UIState.ArtifactInfo,
            UIPanel.InitArgs.CreateWithId(_atifactId));
    }

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += ShowInfo;
        Debug.Log($"Can interact with {Name}!");
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
        Debug.Log($"Cannot interact with {Name} anymore!");
    }
}

[System.Serializable]
public class ArtifactInfo
{
    public int Id;
    public string Name;
    public string Description;
    public string ImagePath;
}
