public static class InventoryService
{
    private static Inventory _inventory;

    public static Inventory Inventory
    {
        get
        {
            if (_inventory == null)
                InitializeInventory();
            return _inventory;
        }
    }

    public static void InitializeInventory()
    {
        _inventory = new Inventory();
        _inventory.Initialize();
    }
}
