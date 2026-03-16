using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {       
        UpdateSafeBanner();
   //     GameAction.Instance.RegisterAction(TypeAction.UpdateAds, UpdateSafeBanner);
    }
    public void UpdateSafeBanner()
    {
        var target = GetComponent<RectTransform>();
        Vector2 size = target.sizeDelta;
        //  var isShowAds = Advertisements.Instance.CanShowAds();
        var isShowAds = true;
        float height = 0;
        if (isShowAds)
        {
          //  height = Advertisements.Instance.GetBannerHeight();
              if (height <= 150)
                height = 150;
        }
        size.y = height;
        target.sizeDelta = size;
    }
}
