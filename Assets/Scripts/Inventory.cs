using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<int> _collectedArtifactsIds = new List<int>();

    public void Initialize()
    {
        ArtifactInfoPanel.OnCollect += AddArtifact;
        _collectedArtifactsIds = PlayerPrefsService.LoadCollectedArtifacts();
        Debug.Log("Inventory initialized");
    }

    private void AddArtifact(ArtifactInfo info)
    {
        if (_collectedArtifactsIds.Contains(info.Id))
        {
            Debug.LogFormat("You already know about {0}", info.Name);
            return;
        }
        Save(info);
    }

    private void Save(ArtifactInfo info)
    {
        _collectedArtifactsIds.Add(info.Id);
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifactsIds);
        Debug.LogFormat("Saved {0} in inventory!", info.Name);
    }
}
