using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<int> _collectedArtifactsIds;

    public void Initialize()
    {
        if (_collectedArtifactsIds != null)
            return;

        _collectedArtifactsIds = new List<int>();
        _collectedArtifactsIds = PlayerPrefsService.LoadCollectedArtifactsIds();
        Debug.Log("Inventory initialized");
    }

    public void AddArtifact(int id)
    {
        if (HasArtifact(id))
        {
            Debug.Log("Already knows about this artifact");
            return;
        }

        var info = ArtifactsService.GetArtifactInfoById(id);
        Debug.Log($"Learned about {info.Name}!");
        _collectedArtifactsIds.Add(id);
        Save();
    }

    private void Save()
    {
        _collectedArtifactsIds.Sort();
        string idString = string.Empty;
        foreach (int id in _collectedArtifactsIds)
        {
            idString += $"{id} ";
        }
        Debug.Log($"Collected artifacts ids in order: {idString}!");
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifactsIds);
    }

    public bool HasArtifact(int id)
    {
        return _collectedArtifactsIds.Contains(id);
    }

    public List<int> GetCollectedArtifactsIds()
    {
        return _collectedArtifactsIds;
    }
}
