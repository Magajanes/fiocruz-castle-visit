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
            if (ArtifactsService.TryGetArtifactInfo(id, out ArtifactInfo info))
            {
                slots[id - 1].Initialize(id);
            }
        }
    }
}
