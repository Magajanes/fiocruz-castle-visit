using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class SoundsManager : GameSingleton<SoundsManager>
{
    public enum ChannelType
    {
        Music = 0,
        Ambience = 1,
        SFX1 = 2,
        SFX2 = 3
    }

    private Dictionary<ChannelType, AudioSource> _channels = new Dictionary<ChannelType, AudioSource>();
    private Coroutine _fadeCoroutine;

    [SerializeField]
    private AudioSource[] _audioChannels;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            _channels[(ChannelType)i] = _audioChannels[i];
        }
    }

    public static void LoadSoundsBundle(string path, Action<SoundsBundle> onLoadFinish)
    {
        ResourceRequest request = Resources.LoadAsync<SoundsBundle>(path);
        request.completed += operation =>
        {
            var bundle = request.asset as SoundsBundle;
            onLoadFinish?.Invoke(bundle);
        };
    }

    public void FadeOutMusic(Action onFadeFinish)
    {
        if (_fadeCoroutine != null) return;

        _fadeCoroutine = StartCoroutine(FadeOutCoroutine(onFadeFinish));
    }

    public void Play(AudioClip clip, ChannelType channelType)
    {
        _channels[channelType].clip = clip;
        _channels[channelType].Play();
    }

    public void Stop(ChannelType channelType)
    {
        _channels[channelType].Stop();
        _channels[channelType].clip = null;
    }

    public void PlayMusic(AudioClip clip)
    {
        _channels[ChannelType.Music].clip = clip;
        _channels[ChannelType.Music].Play();
    }

    public void PlayAmbience(AudioClip clip)
    {
        _channels[ChannelType.Ambience].clip = clip;
        _channels[ChannelType.Ambience].Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!_channels[ChannelType.SFX1].isPlaying)
        {
            _channels[ChannelType.SFX1].PlayOneShot(clip);
            return;
        }

        _channels[ChannelType.SFX2].PlayOneShot(clip);
    }

    private IEnumerator FadeOutCoroutine(Action onFadeFinish)
    {
        float currentVolume = 1;
        while (currentVolume > 0)
        {
            currentVolume -= 0.5f * Time.deltaTime;
            _channels[ChannelType.Music].volume = currentVolume;
            yield return null;
        }

        Stop(ChannelType.Music);
        _channels[ChannelType.Music].volume = 1;
        _fadeCoroutine = null;

        onFadeFinish?.Invoke();
    }
}
