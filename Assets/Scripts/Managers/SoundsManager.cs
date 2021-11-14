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

    public void FadeOutMusic(ChannelType channelType, Action onFadeFinish)
    {
        if (_fadeCoroutine != null) return;

        _fadeCoroutine = StartCoroutine(FadeOutCoroutine(channelType, onFadeFinish));
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

    public void PlayMusic(AudioClip clip, bool loop)
    {
        _channels[ChannelType.Music].clip = clip;
        _channels[ChannelType.Music].Play();
        _channels[ChannelType.Music].loop = loop;
    }

    public void PlayAmbience(AudioClip clip, bool loop)
    {
        _channels[ChannelType.Ambience].clip = clip;
        _channels[ChannelType.Ambience].Play();
        _channels[ChannelType.Ambience].loop = loop;
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

    public void PlaySFXLoop(AudioClip clip, out ChannelType channelType)
    {
        if (!_channels[ChannelType.SFX1].isPlaying)
        {
            channelType = ChannelType.SFX1;
            _channels[channelType].clip = clip;
            _channels[channelType].Play();
            _channels[channelType].loop = true;
            return;
        }

        channelType = ChannelType.SFX2;
        _channels[channelType].clip = clip;
        _channels[channelType].Play();
        _channels[channelType].loop = true;
    }

    public void StopSFXLoop(ChannelType channelType)
    {
        _channels[channelType].Stop();
    }

    public void StopAllSounds()
    {
        foreach (AudioSource channel in _channels.Values)
        {
            if (channel.clip != null)
            {
                channel.Stop();
                channel.clip = null;
            }
        }
    }

    public void BackToMainMenu(Action onFadeFinish)
    {
        FadeOutMusic(ChannelType.Ambience, onFadeFinish);
    }

    private IEnumerator FadeOutCoroutine(ChannelType channelType, Action onFadeFinish)
    {
        float currentVolume = 1;
        while (currentVolume > 0)
        {
            currentVolume -= 0.5f * Time.deltaTime;
            _channels[channelType].volume = currentVolume;
            yield return null;
        }

        Stop(channelType);
        _channels[channelType].volume = 1;
        _fadeCoroutine = null;

        onFadeFinish?.Invoke();
    }
}
