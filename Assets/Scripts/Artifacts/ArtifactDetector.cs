using System;
using UnityEngine;

public class ArtifactDetector : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    public static event Action OnArtifactRegionEnter;
    public static event Action OnArtifactRegionExit;

    private void OnTriggerEnter(Collider other)
    {
        _particleSystem.Play();
        OnArtifactRegionEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _particleSystem.Stop();
        OnArtifactRegionExit?.Invoke();
    }
}
