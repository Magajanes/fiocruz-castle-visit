using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    private Inventory _inventory;
    
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(InitArgs args)
    {
        _inventory = InventoryService.GetInventory();
        var collectedArtifactsIds = _inventory.GetCollectedArtifactsIds();
        foreach (int id in collectedArtifactsIds)
        {
            if (ArtifactsService.TryGetArtifactInfo(id, out ArtifactInfo info))
            {
                slots[id - 1].Initialize(info, true);
            }
        }
    }
}
