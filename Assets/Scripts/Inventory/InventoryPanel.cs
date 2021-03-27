using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(InitArgs args = null)
    {
        var collectedArtifactsIds = InventoryManager.PlayerInventory.GetCollectedArtifactsIds();
        foreach (int id in collectedArtifactsIds)
        {
            slots[id - 1].Initialize(id);
        }
    }
}
