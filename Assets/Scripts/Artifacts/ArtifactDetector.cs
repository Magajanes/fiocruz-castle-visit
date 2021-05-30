using UnityEngine;

public class ArtifactDetector : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;

    private void OnTriggerEnter(Collider other)
    {
        _particleSystem.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _particleSystem.Stop();
    }
}
