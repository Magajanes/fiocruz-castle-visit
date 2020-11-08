using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : UIPanel
{
    private Inventory _inventory;
    
    [SerializeField]
    private List<InventorySlot> slots = new List<InventorySlot>();

    public override void Initialize(object context = null)
    {
        _inventory = context as Inventory;
    }
}
