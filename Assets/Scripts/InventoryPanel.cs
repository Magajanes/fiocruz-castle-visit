using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    private Inventory _inventory;

    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(object context)
    {
        //var collectedArtifactsIds = PlayerPrefsService.LoadCollectedArtifactsIds();
        //foreach (var id in collectedArtifactsIds)
        //{
        //    slots[id - 1].Initialize(true);
        //}
    }
}
