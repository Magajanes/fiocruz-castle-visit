using System.Collections.Generic;
using UnityEngine;

public class ArtifactsImporter : MonoBehaviour
{
    [SerializeField]
    private GameArtifacts _origin;

    [SerializeField]
    private GameArtifacts _overrided;

    [SerializeField]
    private List<int> _itemsToRemove;

    private void Start()
    {
        _overrided.Artifacts.Clear();
        int idToAdd = 1;

        for (int i = 0; i < _origin.Artifacts.Count; i++)
        {
            if (_itemsToRemove.Contains(_origin.Artifacts[i].Id)) continue;

            _overrided.Artifacts.Add(ArtifactInfo.CopyAndOverrideId(_origin.Artifacts[i], idToAdd));
            idToAdd++;
        }
    }
}
