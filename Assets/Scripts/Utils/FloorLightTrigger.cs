using UnityEngine;

public class FloorLightTrigger : LightTrigger, ILightTrigger
{
    [SerializeField]
    private int _floor;

    public void Initialize()
    {
        OnLightTriggerEnter += () =>
        {
            LightManager.Instance.SetCurrentFloor(_floor);
        };
    }
}
