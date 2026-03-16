using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveFromX : MonoBehaviour
{
    Vector3 startPos = Vector3.zero;
    public Ease Ease;
    public float time = 0.3f;
    public Vector3 offset;
    public float delay = 0;
    bool init = false;
    System.Action actionUpdate = null;
    private void OnEnable()
    {
        StartAnim();
    }
    public void StartAnim()
    {
        if(!init)
        {
            init = true;
            startPos = transform.GetComponent<RectTransform>().anchoredPosition;
        }
          float t = 0;
        var rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = startPos + offset;
      
        actionUpdate = () =>
        {
            t+=Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, startPos, t);
            if(t>=1)
            {
                rectTransform.anchoredPosition = startPos;
                actionUpdate = null;
            }
        };
      //  transform.DOLocalMove(startPos, time).SetEase(Ease).SetDelay(delay);
    }
    void Update()
    {
        actionUpdate?.Invoke();
    }
}
