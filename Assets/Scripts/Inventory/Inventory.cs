using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, bool> _collectedArtifacts = new Dictionary<int, bool>();

    public void Initialize()
    {
        _collectedArtifacts = PlayerPrefsService.LoadCollectedArtifacts();
        Debug.Log("Inventory initialized");
    }

    public void AddArtifact(int id)
    {
        if (HasArtifact(id))
            return;

        ArtifactInfo info = ArtifactsService.GetArtifactInfoById(id);
        Debug.Log($"Learned about {info.Name}!");
        _collectedArtifacts[id] = true;
        Save();
    }

    private void Save()
    {
        PlayerPrefsService.SaveCollectedArtifacts(_collectedArtifacts);
    }

    public bool HasArtifact(int id)
    {
        if (!_collectedArtifacts.ContainsKey(id))
        {
            Debug.Log($"Artifact with id: {id} does not exist!");
            return false;
        }

        if (!_collectedArtifacts[id])
        {
            ArtifactInfo info = ArtifactsService.GetArtifactInfoById(id);
            Debug.Log($"You still don't know about artifact {info.Name}! Artifact id: {id}");
        }

        return _collectedArtifacts[id];
    }

    public List<int> GetSortedCollectedArtifactsIds()
    {
        List<int> collectedArtifactsIds = new List<int>();
        foreach (KeyValuePair<int, bool> pair in _collectedArtifacts)
        {
            if (pair.Value)
                collectedArtifactsIds.Add(pair.Key);
        }
        collectedArtifactsIds.Sort();
        return collectedArtifactsIds;
    }
}
