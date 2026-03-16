using UnityEngine;
using DG.Tweening;

public class ZoomEffect : MonoBehaviour
{
    [SerializeField] private float zoomScale = 1.2f;   // How much to zoom
    [SerializeField] private float duration = 0.5f;    // Duration of zoom in/out

    void Start()
    {
        // Save original scale
        Vector3 originalScale = transform.localScale;
        Vector3 zoomedScale = originalScale * zoomScale;
        // Create the zoom in/out loop
        transform.DOScale(zoomedScale, duration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }
}