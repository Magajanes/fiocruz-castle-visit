using System;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _mainHallLights;
    [SerializeField]
    private RegionDoor[] _mainDoors;

    private Dictionary<int, Action<bool>> _lightActions = new Dictionary<int, Action<bool>>();

    public void Initialize()
    {
        RegionDoor.OnRegionEnter += OnRegionEnter;

        _lightActions.Add(0, TurnMainHallLights);
        Debug.Log("Light manger initialized!");
    }

    private void OnRegionEnter(int regionId)
    {
        if (!_lightActions.ContainsKey(regionId))
            return;
        
        _lightActions[regionId].Invoke(true);
    }

    private void OnMainRegionExit()
    {
        if (!_lightActions.ContainsKey(0))
            return;

        _lightActions[0].Invoke(false);
    }

    private void TurnMainHallLights(bool on)
    {
        foreach (GameObject light in _mainHallLights)
        {
            light.SetActive(on);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (RegionDoor door in _mainDoors)
        {
            door.CloseDoor(OnMainRegionExit);
        }
    }
}
