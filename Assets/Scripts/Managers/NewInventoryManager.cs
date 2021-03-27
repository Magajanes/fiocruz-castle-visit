using UnityEngine;

public class NewInventoryManager : MonoBehaviour
{
    public static Inventory PlayerInventory;

    public void Initialize()
    {
        if (PlayerInventory != null) return;

        PlayerInventory = new Inventory();
        PlayerInventory.Initialize();
    }
}
