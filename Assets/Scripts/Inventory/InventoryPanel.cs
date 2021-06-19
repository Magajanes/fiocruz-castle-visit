using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public void Initialize()
    {
        var inventory = InventoryManager.PlayerInventory;
        List<int> collectedArtifactsIds = inventory.GetSortedCollectedArtifactsIds();
        foreach (int id in collectedArtifactsIds)
        {
            slots[id - 1].Initialize(id);
        }
    }
}
