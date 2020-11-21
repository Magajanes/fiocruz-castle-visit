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
            return;

        _collectedArtifactsIds.Add(id);
        Save();
    }

    private void Save()
    {
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifactsIds);
    }

    public bool HasArtifact(int id)
    {
        Debug.Log("Already knows about this artifact");
        return _collectedArtifactsIds.Contains(id);
    }

    public List<int> GetCollectedArtifactsIds()
    {
        return _collectedArtifactsIds;
    }
}
