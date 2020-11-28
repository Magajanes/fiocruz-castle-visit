using System.Collections.Generic;

public static class InventoryService
{
    private static Inventory _inventory;

    public static void InitializeInventory()
    {
        _inventory = new Inventory();
        _inventory.Initialize();
    }

    public static void SaveArtifact(int artifactId)
    {
        _inventory.AddArtifact(artifactId);
    }

    public static int GetArtifactIndex(int id)
    {
        List<int> collectedArtifactsIds = _inventory.GetCollectedArtifactsIds();
        return collectedArtifactsIds.IndexOf(id);
    }

    public static List<int> GetCollectedArtifactsIds()
    {
        return _inventory.GetCollectedArtifactsIds();
    }
}
