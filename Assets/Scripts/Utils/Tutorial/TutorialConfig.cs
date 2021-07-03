using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialConfig", menuName = "Tutorial", order = 0)]
public class TutorialConfig : ScriptableObject
{
    public List<TutorialConfigEntry> TutorialEntries;
}

[System.Serializable]
public struct TutorialConfigEntry
{
    public TutorialSubject Subject;
    public string Message;
}
