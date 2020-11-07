using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField]
    private Color notCollectedColor;
    [SerializeField]
    private Color collectedColor = Color.white;
    [Header("References")]
    [SerializeField]
    private Image artifactImage;

    public void Initialize(bool isArtifactCollected)
    {
        artifactImage.color = isArtifactCollected ? collectedColor : notCollectedColor;
    }
}
