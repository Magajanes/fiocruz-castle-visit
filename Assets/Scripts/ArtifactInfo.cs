﻿using UnityEngine;

[CreateAssetMenu(fileName = "NewArtifactInfo", menuName = "Artifacts/Info", order = 0)]
public class ArtifactInfo : ScriptableObject
{
    public int Id;
    public string Name;
    public string Description;
    public Sprite Image;
    public InventoryEntry Entry;
}

[System.Serializable]
public class InventoryEntry
{
    public int Id;
    public string Name;
    public string Description;
    public Sprite Image;
}
