using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameArtifacts", menuName = "Artifacts", order = 0)]
public class GameArtifacts : ScriptableObject
{
    public List<ArtifactInfo> Artifacts;
}
