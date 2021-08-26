using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundsManager : GameSingleton<SoundsManager>
{
    public enum SoundChannel
    {
        Music = 0,
        Ambience = 1,
        SFX1 = 2,
        SFX2 = 3
    }

    private Dictionary<SoundChannel, AudioSource> _channels = new Dictionary<SoundChannel, AudioSource>();
    private Coroutine _fadeCoroutine;

    [SerializeField]
    private AudioSource[] _audioChannels;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            _channels[(SoundChannel)i] = _audioChannels[i];
        }
    }

    public void PlayIntro()
    {
        ResourceRequest request = Resources.LoadAsync("Music/full");
        request.completed += (operation) =>
        {
            AudioClip clip = request.asset as AudioClip;
            _channels[SoundChannel.Music].clip = clip;
            _channels[SoundChannel.Music].Play();
        };
    }

    public void StopIntro()
    {
        if (_fadeCoroutine != null) return;

        _fadeCoroutine = StartCoroutine(FadeOutMusic());
    }

    public void Play(AudioClip clip, SoundChannel channel)
    {
        _channels[channel].clip = clip;
        _channels[channel].Play();
    }

    public void Stop(SoundChannel channel)
    {
        _channels[channel].Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
        _channels[SoundChannel.Music].clip = clip;
        _channels[SoundChannel.Music].Play();
    }

    public void PlayAmbience(AudioClip clip)
    {
        _channels[SoundChannel.Ambience].clip = clip;
        _channels[SoundChannel.Ambience].Play();
    }

    private IEnumerator FadeOutMusic()
    {
        float currentVolume = 1;
        while (currentVolume > 0)
        {
            currentVolume -= 0.5f * Time.deltaTime;
            _channels[SoundChannel.Music].volume = currentVolume;
            yield return null;
        }
        Stop(SoundChannel.Music);
        _channels[SoundChannel.Music].volume = 1;
        _fadeCoroutine = null;
    }
}
