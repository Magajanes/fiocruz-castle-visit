using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    private ArtifactsService _artifactsService;
    private Inventory _inventory;
    
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public void Initialize(ArtifactsService artifactsService, Inventory inventory)
    {
        _inventory = inventory;
        _artifactsService = artifactsService;
    }

    public override void Show(int artifactId)
    {
        var collectedArtifactsIds = _inventory.GetCollectedArtifactsIds();
        foreach (var id in collectedArtifactsIds)
        {
            var artifactInfo = _artifactsService.GetArtifactInfoById(id);
            slots[id - 1].Initialize(artifactInfo, true);
        }
    }
}
