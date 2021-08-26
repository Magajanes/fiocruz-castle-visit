using UnityEngine;

public class SoundsManager : GameSingleton<SoundsManager>
{
    [SerializeField]
    private AudioSource _audioSource;

    public void PlayIntro()
    {
        ResourceRequest request = Resources.LoadAsync("Music/full");
        request.completed += (operation) =>
        {
            AudioClip clip = request.asset as AudioClip;
            _audioSource.clip = clip;
            _audioSource.Play();
        };
    }
}
