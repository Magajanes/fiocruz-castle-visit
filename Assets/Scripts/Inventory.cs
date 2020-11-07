using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<int> _collectedArtifactsIds;

    public void Initialize()
    {
        if (_collectedArtifactsIds != null)
            return;

        ArtifactInfoPanel.OnCollect += AddArtifact;
        _collectedArtifactsIds = new List<int>();
        _collectedArtifactsIds = PlayerPrefsService.LoadCollectedArtifactsIds();
        Debug.Log("Inventory initialized");
    }

    private void AddArtifact(ArtifactInfo info)
    {
        if (_collectedArtifactsIds.Contains(info.Id))
        {
            Debug.Log($"You already know about {info.Name}");
            return;
        }
        _collectedArtifactsIds.Add(info.Id);
        Save(info);
    }

    private void Save(ArtifactInfo info)
    {
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifactsIds);
        Debug.Log($"Saved {info.Name} in inventory!");
    }
}
