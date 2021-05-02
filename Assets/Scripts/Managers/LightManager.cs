using System.Collections.Generic;
using UnityEngine;

public class LightManager : Singleton<LightManager>
{
    [Header("Light Triggers")]
    [SerializeField]
    private GameObject[] _lightTriggers;
    
    [Header("Light Objects")]
    [SerializeField]
    private GameObject[] _mainHallLights;
    [SerializeField]
    private GameObject[] _firstFloorLights;
    [SerializeField]
    private GameObject[] _secondFloorLights;
    [SerializeField]
    private GameObject[] _thirdFloorLights;
    [SerializeField]
    private GameObject[] _fourthFloorLights;

    private bool _isAtMainHall;
    private int _currentFloor;
    private Dictionary<int, GameObject[]> _lightsByFloor = new Dictionary<int, GameObject[]>();

    public bool IsAtMainHall => _isAtMainHall;

    private void Start()
    {
        InitializeLightTriggers();
        InitializeFloorsDictionary();
    }

    private void InitializeLightTriggers()
    {
        foreach (GameObject lightTrigger in _lightTriggers)
        {
            var trigger = lightTrigger.GetComponent<ILightTrigger>();
            trigger.Initialize();
        }
    }

    private void InitializeFloorsDictionary()
    {
        _lightsByFloor[1] = _firstFloorLights;
        _lightsByFloor[2] = _secondFloorLights;
        _lightsByFloor[3] = _thirdFloorLights;
        _lightsByFloor[4] = _fourthFloorLights;
    }

    public void SetIsAtMainHall(bool isAtMainHall)
    {
        _isAtMainHall = isAtMainHall;
        SetMainHallLights();
        if (!_isAtMainHall)
        {
            SetCurrentFloorLights();
        }
    }

    public void SetCurrentFloor(int currentFloor)
    {
        _currentFloor = currentFloor;
        SetCurrentFloorLights();
        SetMainHallLights();
    }

    private void SetMainHallLights()
    {
        foreach (GameObject lightObject in _mainHallLights)
        {
            lightObject.SetActive(_isAtMainHall);
        }
    }

    private void SetCurrentFloorLights()
    {
        foreach (int floor in _lightsByFloor.Keys)
        {
            foreach (GameObject light in _lightsByFloor[floor])
            {
                light.SetActive(floor == _currentFloor);
            }
        }
    }
}
