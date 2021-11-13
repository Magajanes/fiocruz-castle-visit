﻿using System;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField]
    private int _atifactId;
    private string _name;
    private string _description;

    private ArtifactInfo Info => ArtifactsService.GetArtifactInfoById(_atifactId);

    public static event Action<InitArgs> OnInteraction;

    public string Name
    {
        get
        {
            if (_name == null)
            {
                if (Info != null)
                {
                    return Info.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _name;
        }
    }

    public string Description
    {
        get
        {
            if (_description == null)
            {
                if (Info != null)
                {
                    return Info.Description;
                }
                else
                {
                    return string.Empty;
                }
            }
            return _description;
        }
    }


    private void ShowInfo()
    {
        var args = InitArgs.CreateWithId(_atifactId);
        OnInteraction?.Invoke(args);
    }

    private void OnTriggerEnter(Collider other)
    {
        TutorialController.Instance.ShowTutorial(TutorialSubject.ArtifactInteraction);
        InputController.OnInteractionButtonPress -= ShowInfo;
        InputController.OnInteractionButtonPress += ShowInfo;
    }

    private void OnTriggerExit(Collider other)
    {
        TutorialController.Instance.HideTutorial(TutorialSubject.ArtifactInteraction);
        InputController.OnInteractionButtonPress -= ShowInfo;
    }

    private void OnDestroy()
    {
        InputController.OnInteractionButtonPress -= ShowInfo;
    }
}

[System.Serializable]
public class ArtifactInfo
{
    public int Id;
    public string Name;
    public string Description;
    public string ImagePath;
}
