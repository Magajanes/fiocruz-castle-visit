using UnityEngine;

public class InGameSoundsManager : Singleton<InGameSoundsManager>
{
    public const string INPUT_SOUNDS_BUNDLE_PATH = "SoundBundles/InGameSounds";
    public const string ARTIFACT_FOUND_ID = "artifact_found";
    public const string OPEN_DOOR_ID = "open_door";

    private SoundsBundle _soundsBundle;
    private AudioClip _artifactFoundSound;
    private AudioClip _openDoorSound;
    private SoundsManager.ChannelType _currentChannel;

    private void Start()
    {
        SoundsManager.LoadSoundsBundle(INPUT_SOUNDS_BUNDLE_PATH, OnSoundsLoaded);

        void OnSoundsLoaded(SoundsBundle bundle)
        {
            _soundsBundle = bundle;
            _artifactFoundSound = _soundsBundle.GetAudioClipById(ARTIFACT_FOUND_ID);
            _openDoorSound = _soundsBundle.GetAudioClipById(OPEN_DOOR_ID);
            AddListeners();
        }
    }

    private void AddListeners()
    {
        Door.OnDoorOpen -= PlayOpenDoorSound;
        Door.OnDoorOpen += PlayOpenDoorSound;

        ArtifactDetector.OnArtifactRegionEnter -= PlayArtifactFoundLoop;
        ArtifactDetector.OnArtifactRegionEnter += PlayArtifactFoundLoop;

        ArtifactDetector.OnArtifactRegionExit -= StopArtifactFoundLoop;
        ArtifactDetector.OnArtifactRegionExit += StopArtifactFoundLoop;
    }

    private void PlayOpenDoorSound()
    {
        SoundsManager.Instance.PlaySFX(_openDoorSound);
    }

    private void PlayArtifactFoundLoop()
    {
        SoundsManager.Instance.PlaySFXLoop(_artifactFoundSound, out _currentChannel);
    }

    private void StopArtifactFoundLoop()
    {
        SoundsManager.Instance.StopSFXLoop(_currentChannel);
    }
}
