using System;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    protected Action OnLightTriggerEnter; 
    
    private void OnTriggerEnter(Collider other)
    {
        OnLightTriggerEnter?.Invoke();
    }
}

public interface ILightTrigger
{
    void Initialize();
}
