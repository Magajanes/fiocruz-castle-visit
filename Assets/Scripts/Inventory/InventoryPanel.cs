using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(InitArgs args)
    {
        Inventory _inventory = InventoryService.GetInventory();
        List<int> collectedArtifactsIds = _inventory.GetCollectedArtifactsIds();
        foreach (int id in collectedArtifactsIds)
        {
            if (ArtifactsService.TryGetArtifactInfo(id, out ArtifactInfo info))
            {
                slots[id - 1].Initialize(id);
            }
        }
    }
}
