using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    //TODO - Create serializable data structure to handle inventory entry
    private Dictionary<int, ArtifactInfo> _collectedArtifacts = new Dictionary<int, ArtifactInfo>();

    public void Initialize()
    {
        ArtifactInfoPanel.OnCollect += AddArtifact;
        _collectedArtifacts = PlayerPrefsService.LoadCollectedArtifacts();
        Debug.Log("Inventory initialized");
    }

    private void AddArtifact(ArtifactInfo info)
    {
        if (_collectedArtifacts.ContainsKey(info.Id))
        {
            Debug.LogFormat("You already know about {0}", info.Name);
            return;
        }

        Save(info);
    }

    private void Save(ArtifactInfo info)
    {
        _collectedArtifacts.Add(info.Id, info);
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifacts);
        Debug.LogFormat("Saved {0} in inventory!", info.Name);
    }
}
