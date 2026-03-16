using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ZoomOut : MonoBehaviour
{
    Vector3 startSize = Vector3.zero;
    public float time = 0.25f;
    public Ease Ease;
    private void OnEnable()
    {
        if(Vector3.Magnitude(startSize)==0)
        {
            startSize = transform.localScale;
        }
        transform.localScale = Vector3.zero;
        transform.DOScale(startSize,time).SetEase(Ease);
    }
    private void OnDisable()
    {
        if (Vector3.Magnitude(startSize) == 0)
        {
            startSize = transform.localScale;
        }
    }
}
