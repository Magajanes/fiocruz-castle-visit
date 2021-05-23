using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    
    [SerializeField]
    private int _atifactId;
    private string _name;
    private string _description;

    private ArtifactInfo Info => ArtifactsService.GetArtifactInfoById(_atifactId);

    public string Name
    {
        get
        {
            if (_name == null)
            {
                if (Info != null)
                {
                    return Info.Name;
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
                if (Info != null)
                {
                    return Info.Description;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _description;
        }
    }


    private void ShowInfo()
    {
        UIManager.ChangeState(
            UIState.ArtifactInfo,
            UIPanel.InitArgs.CreateWithId(_atifactId)
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        InputController.OnInteractionButtonPress += ShowInfo;
        _particleSystem.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
        _particleSystem.Stop();
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
