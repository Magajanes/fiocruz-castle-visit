using UnityEngine;

public class SpecialLightTrigger : LightTrigger, ILightTrigger
{
    [SerializeField]
    private GameObject[] _lights;
    
    public void Initialize()
    {
        OnLightTriggerEnter += () =>
        {
            foreach (GameObject light in _lights)
            {
                light.SetActive(false);
            }
        };
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject light in _lights)
        {
            light.SetActive(true);
        }
    }
}
