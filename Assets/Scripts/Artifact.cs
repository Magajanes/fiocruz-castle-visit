using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private int _atifactId;
    private string _name;
    private string _description;
    private string _imagePath;

    public string Name
    {
        get
        {
            if (_name == null)
            {
                if (ArtifactsService.TryGetArtifactName(_atifactId, ref _name))
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
                if (ArtifactsService.TryGetArtifactDescription(_atifactId, ref _description))
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

    public string ImagePath
    {
        get
        {
            if (_imagePath == null)
            {
                if (ArtifactsService.TryGetArtifactImagePath(_atifactId, ref _imagePath))
                {
                    return _imagePath;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _imagePath;
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
