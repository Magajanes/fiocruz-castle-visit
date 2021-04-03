using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        LoadUIManager();
    }

    private void LoadUIManager()
    {
        ResourceRequest request = Resources.LoadAsync("Prefabs/UIManager");
        request.completed += InstantiateUIManager;

        void InstantiateUIManager(AsyncOperation operation)
        {
            var prefab = request.asset as GameObject;
            var uiManager = Instantiate(prefab).GetComponent<UIManager>();
            uiManager.transform.SetParent(transform.parent);
            uiManager.Initialize();
            InputController.Instance.LockInputs(false);
            InputController.Instance.SetInputScheme(UIState.Inactive);
        }
    }
}
