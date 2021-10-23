using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundsBundle", menuName = "Bundles/Sounds", order = 0)]
public class SoundsBundle : ScriptableObject
{
    [SerializeField]
    private List<SoundAsset> _soundAssets;

    public AudioClip GetAudioClipById(string clipId)
    {
        SoundAsset soundAsset = _soundAssets.Find(asset => asset.Id == clipId);
        if (soundAsset == null)
        {
            Debug.LogError($"Couldn't find clip with id {clipId}!");
            return null;
        }

        return soundAsset.Clip;
    }
}

[System.Serializable]
public class SoundAsset
{
    public string Id;
    public AudioClip Clip;
}
