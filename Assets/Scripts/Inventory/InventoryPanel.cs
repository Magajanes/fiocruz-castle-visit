using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(InitArgs args)
    {
        List<int> collectedArtifactsIds = InventoryService.GetCollectedArtifactsIds();
        foreach (int id in collectedArtifactsIds)
        {
            ArtifactInfo artifact = ArtifactsService.GetArtifactInfoById(id);
            if (artifact != null)
                slots[id - 1].Initialize(id);
        }
    }
}
