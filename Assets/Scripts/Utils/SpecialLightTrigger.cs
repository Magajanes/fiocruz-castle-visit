using UnityEngine;

public class SpecialLightTrigger : LightTrigger, ILightTrigger
{
    [SerializeField]
    private GameObject _light;
    
    public void Initialize()
    {
        OnLightTriggerEnter += () =>
        {
            _light.SetActive(false);
        };
    }

    private void OnTriggerExit(Collider other)
    {
        _light.SetActive(true);
    }
}
