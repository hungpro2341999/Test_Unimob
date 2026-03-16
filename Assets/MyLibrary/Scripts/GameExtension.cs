using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
[System.Serializable]
public class Config
{
}

public static class TransformExtension
{
    public static void StretchFull(this RectTransform rect)
{
    rect.anchorMin = Vector2.zero;
    rect.anchorMax = Vector2.one;
    rect.offsetMin = Vector2.zero;
    rect.offsetMax = Vector2.zero;
}
}