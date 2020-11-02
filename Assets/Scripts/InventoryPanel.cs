using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    private Inventory _inventory;

    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(object context)
    {
        _inventory = Player.Inventory;
        var collectedArtifactsIds = PlayerPrefsService.LoadCollectedArtifactsIds();
        foreach (var id in collectedArtifactsIds)
        {
            var entry = _inventory.LoadEntry(id);
            slots[id - 1].Initialize(entry);
        }
    }
}
