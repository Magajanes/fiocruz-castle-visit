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

        ArtifactInfo info = ArtifactsService.GetArtifactInfoById(id);
        Debug.Log($"Learned about {info.Name}!");
        _collectedArtifactsIds.Add(id);
        Save();
    }

    private void Save()
    {
        _collectedArtifactsIds.Sort();
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifactsIds);
    }

    public bool HasArtifact(int id)
    {
        if (_collectedArtifactsIds.Contains(id))
        {
            Debug.Log("Already knows about this artifact");
            return true;
        }

        return false;
    }

    public int GetArtifactIndex(int id)
    {
        return _collectedArtifactsIds.IndexOf(id);
    }

    public List<int> GetCollectedArtifactsIds()
    {
        return _collectedArtifactsIds;
    }
}
