using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHallLightTrigger : LightTrigger, ILightTrigger
{
    public void Initialize()
    {
        OnLightTriggerEnter += () =>
        {
            LightManager.Instance.SetIsAtMainHall(true);
        };
    }

    private void OnTriggerExit(Collider other)
    {
        LightManager.Instance.SetIsAtMainHall(false);
    }
}
