using UnityEngine;

public class Player : MonoBehaviour
{
    private static Inventory _inventory;

    private void Start()
    {
        if (_inventory != null)
            return;
        _inventory = new Inventory();
        _inventory.Initialize();
    }
}
