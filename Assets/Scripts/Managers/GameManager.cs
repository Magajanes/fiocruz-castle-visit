using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _deletePlayerPrefs;

    private void Awake()
    {
        if (_deletePlayerPrefs)
            PlayerPrefsService.DeletePlayerPrefs();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InputController.Instance.LockInputs(false);
        InputController.Instance.SetInputScheme(UIState.Inactive);
        //LoadUIManager();
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
